using ECommerce.Config;
using ECommerce.Hubs;
using ECommerce.Models.DTOs.Cart;
using ECommerce.Models.DTOs.Color;
using ECommerce.Models.DTOs.Order;
using ECommerce.Models.DTOs.Product;
using ECommerce.Models.DTOs.Review;
using ECommerce.Models.DTOs.Size;
using ECommerce.Models.DTOs.User;
using ECommerce.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Data.Repositories.Customer;

public class CustomerRepository : BaseRepository, ICustomerRepository
{
    private readonly UserManager<User> _userManager;
    private readonly IHubContext<OrderHub> _orderHubContext;
        
    public CustomerRepository(ProductDbContext context, UserManager<User> userManager, IHubContext<OrderHub> orderHubContext) : base(context)
    {
        _userManager = userManager;
        _orderHubContext = orderHubContext;
    }

    public async Task AddToWishlist(string userId, Guid productId)
    {
        await GetAuthorizedUserAsync(userId);
        var product = await _db.Products
            .AsNoTracking()
            .AnyAsync(p => p.Id == productId);
        
        if (!product) throw new ArgumentException("Product not found");

        var wish = await _db.Wishlists
            .AnyAsync(w => w.ProductId == productId && w.UserId == userId);
        
        if (wish) throw new ArgumentException("Product already in wishlist");
        await _db.Wishlists.AddAsync(new Wishlist
            {
                UserId = userId,
                ProductId = productId,
            });
        await SaveChangesAsyncWithTransaction();
    }

    public async Task RemoveFromWishlist(string userId, Guid productId)
    {
        await GetAuthorizedUserAsync(userId);

        var product = await _db.Products.AsNoTracking()
            .AnyAsync(p => p.Id == productId);
        
        if (!product) throw new ArgumentException("Product not found");
        
        var wish = await _db.Wishlists
            .FirstOrDefaultAsync(w => w.ProductId == productId && w.UserId == userId);
        
        if (wish == null) throw new ArgumentException("Product not in wishlist");
        
        _db.Wishlists.Remove(wish);
        
        await SaveChangesAsyncWithTransaction();
    }
    
    public async Task<UserReviewsDto> GetReviewsForUser(string userId, IReadOnlyList<string> roles)
    {
        var reviews = await _db.Reviews
            .AsNoTracking()
            .Where(r => r.CustomerId == userId)
            .Select(r => new ReviewDto
            {
                Id = r.Id,
                Title = r.Title,
                Content = r.Content,
                Rating = r.Rating,
                CreatedAt = r.CreatedAt,
                Product = new ReviewProductDto
                {
                    Id = r.Product.Id,
                    Name = r.Product.Name,
                    Description = r.Product.Description,
                    Price = r.Product.Stocks.MinBy(s => s.Price).Price,
                    ImageUrl = r.Product.Images.FirstOrDefault().ImageUrls.FirstOrDefault()
                }
            })
            .ToListAsync();

        var user = await GetAuthorizedUserAsync(userId);
        return new UserReviewsDto
        {
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.FirstName,
                ImageUrl = user.ImageUrl,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                Role = UserRoles.GetHighestUserRole(roles),
                IsBanned = false
            },
            Reviews = reviews
        };
    }

    public async Task RequestUpgradingToSeller(string userId, SellerInfoCreateDto sellerInfo)
    {
        var nameExists = await _db.SellerUpgradeRequests.Where(req =>
                req.Status == SellerUpgradeRequestStatus.Approved && sellerInfo.Name == req.SellerInfo.Name)
            .AnyAsync();

        if (nameExists) throw new ArgumentException(" name already exists");

        var user = await GetAuthorizedUserAsync(userId);
        await CheckIfUserIsSeller(user);

        var seller = new SellerInfo
        {
            Name = sellerInfo.Name,
            Description = sellerInfo.Description,
            Email = sellerInfo.Email,
            Phone = sellerInfo.Phone
        };
        await _db.Sellers.AddAsync(seller);
        await _db.SellerUpgradeRequests.AddAsync(new SellerUpgradeRequest
        {
            UserId = userId,
            SellerInfoId = seller.Id
        });
        await SaveChangesAsyncWithTransaction();
    }

    public async Task<OrderDto> GetCartForUser(string userId)
    {
        var lastOrder = await _db.Orders
            .AsNoTracking()
            .Include(o => o.OrderItems)
            .ThenInclude(orderItem => orderItem.Product)
            .ThenInclude(product => product.Stocks)
            .Include(o => o.OrderItems)
            .ThenInclude(orderItem => orderItem.Product)
            .ThenInclude(product => product.Images).Include(order => order.OrderItems)
            .ThenInclude(orderItem => orderItem.Color).Include(order => order.OrderItems)
            .ThenInclude(orderItem => orderItem.Size).Include(order => order.OrderItems)
            .ThenInclude(orderItem => orderItem.Product).ThenInclude(product => product.Seller)
            .ThenInclude(user => user.SellerInfo)
            .Where(o => o.CustomerId == userId && o.Status == OrderStatus.Pending)
            .OrderByDescending(o => o.CreatedAt)
            .FirstOrDefaultAsync();

        if (lastOrder == null) return new OrderDto();

        return new OrderDto
        {
            Id = lastOrder.Id,
            Products = lastOrder.OrderItems.Select(oi => new OrderItemDto
            {
                Id = oi.Id,
                Product = new OrderItemProductDto
                {
                    Id = oi.Product.Id,
                    Name = oi.Product.Name,
                    Description = oi.Product.Description,
                    Price = oi.Product.Stocks.MinBy(s => s.Price).Price,
                    ImageUrl = oi.Product.Images.FirstOrDefault(i => oi.ColorId == i.ColorId)?.ImageUrls.FirstOrDefault(),
                    Color = new ColorDto
                    {
                        Id = oi.Color.Id,
                        Name = oi.Color.Name,
                        HexCode = oi.Color.HexCode,
                    },
                    Size = new SizeDto
                    {
                        Id = oi.Size.Id,
                        Name = oi.Size.Name,
                        Value = oi.Size.Value
                    },
                    SellerName = oi.Product.Seller?.SellerInfo?.Name ?? "Unknown",
                    SellerId = oi.Product.SellerId
                },
                Quantity = oi.Quantity
            }).ToList(),
            TotalPrice = lastOrder.OrderItems.Sum(oi => oi.Product.Stocks.MinBy(s => s.Price).Price * oi.Quantity),
            TotalQuantity = lastOrder.OrderItems.Sum(oi => oi.Quantity)
        };
    }

    public async Task AddProductToCart(string userId, CreateCartItemDto cartProduct)
    {
        var lastOrderAndProduct = await _db.Orders
            .Include(o => o.OrderItems)
            .Where(o => o.CustomerId == userId && o.Status == OrderStatus.Pending)
            .OrderByDescending(o => o.CreatedAt)
            .Select(o => new 
            {
                Order = o,
                Product = _db.Products
                    .Include(p => p.Stocks)
                    .FirstOrDefault(p => p.Id == cartProduct.ProductId)
            })
            .FirstOrDefaultAsync();

        var lastOrder = lastOrderAndProduct?.Order;
        var product = lastOrderAndProduct?.Product;

        if (product == null)
        {
            throw new ArgumentException("Product not found");
        }

        var stock = product.Stocks.FirstOrDefault(s => s.ColorId == cartProduct.ColorId && s.SizeId == cartProduct.SizeId);

        if (stock == null || stock.Stock < cartProduct.Quantity)
        {
            throw new ArgumentException("Not enough stock");
        }

        if (lastOrder == null)
        {
            var user = await GetAuthorizedUserAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            lastOrder = new Order
            {
                Id = new Guid(),
                CustomerId = userId,
                Status = OrderStatus.Pending
            };
            await _db.Orders.AddAsync(lastOrder);
        }

        var existingProduct = lastOrder.OrderItems.FirstOrDefault(p => p.ProductId == cartProduct.ProductId && p.ColorId == cartProduct.ColorId && p.SizeId == cartProduct.SizeId);
        
        if (existingProduct != null)
        {
            existingProduct.Quantity += cartProduct.Quantity;
        }
        else
        {
            await _db.OrderItems.AddAsync(new OrderItem
            {
                OrderId = lastOrder.Id,
                ProductId = cartProduct.ProductId,
                ColorId = cartProduct.ColorId,
                SizeId = cartProduct.SizeId,
                Quantity = cartProduct.Quantity
            });
        }

        await SaveChangesAsyncWithTransaction();
    }
    public async Task RemoveProductFromCart(string userId, Guid orderItemId)
    {
        var lastOrder = await _db.Orders
            .Include(o => o.OrderItems)
            .Where(o => o.CustomerId == userId && o.Status == OrderStatus.Pending)
            .OrderByDescending(o => o.CreatedAt)
            .FirstOrDefaultAsync();

        if (lastOrder == null) throw new ArgumentException("No pending order found for the user");

        var orderItem = lastOrder.OrderItems.FirstOrDefault(oi => oi.Id == orderItemId);
        if (orderItem == null) throw new ArgumentException("Product not found in the order");

        _db.OrderItems.Remove(orderItem);

        await SaveChangesAsyncWithTransaction();
    }
    public async Task UpdateProductInCart(string userId, Guid orderItemId, ChangeCartItemDto cartProduct)
    {
        var lastOrder = await _db.Orders
            .Include(o => o.OrderItems)
            .Where(o => o.CustomerId == userId && o.Status == OrderStatus.Pending)
            .OrderByDescending(o => o.CreatedAt)
            .FirstOrDefaultAsync();

        if (lastOrder == null) throw new ArgumentException("No pending order found for the user");

        var orderItem = lastOrder.OrderItems.FirstOrDefault(oi => oi.Id == orderItemId);
        if (orderItem == null) throw new ArgumentException("Product not found in the order");

        orderItem.Quantity = cartProduct.Quantity;

        await SaveChangesAsyncWithTransaction();
    }

    public async Task ClearCart(string userId)
    {
        var lastOrder = await _db.Orders
            .Include(o => o.OrderItems)
            .Where(o => o.CustomerId == userId && o.Status == OrderStatus.Pending)
            .OrderByDescending(o => o.CreatedAt)
            .FirstOrDefaultAsync();
        
        if (lastOrder == null) throw new ArgumentException("No pending order found for the user");
        
        _db.OrderItems.RemoveRange(lastOrder.OrderItems);
        
        await SaveChangesAsyncWithTransaction();
    }
    
    public async Task ConfirmCart(string userId)
    {
        var lastOrder = await _db.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .ThenInclude(p => p.Stocks)
            .Where(o => o.CustomerId == userId && o.Status == OrderStatus.Pending)
            .OrderByDescending(o => o.CreatedAt)
            .FirstOrDefaultAsync();
        
        if (lastOrder == null) throw new ArgumentException("No pending order found for the user");
        
        lastOrder.Status = OrderStatus.Accepted;
        
        await SaveChangesAsyncWithTransaction();
        
    }
    
    public async Task<List<OrderDto>> GetOrdersForUser(string userId)
    {
        var orders = await _db.Orders
            .AsNoTracking()
            .Include(o => o.OrderItems)
            .ThenInclude(orderItem => orderItem.Product)
            .ThenInclude(product => product.Stocks)
            .Include(o => o.OrderItems)
            .ThenInclude(orderItem => orderItem.Product)
            .ThenInclude(product => product.Images)
            .Include(order => order.OrderItems)
            .ThenInclude(orderItem => orderItem.Color)
            .Include(order => order.OrderItems)
            .ThenInclude(orderItem => orderItem.Size)
            .Include(order => order.OrderItems)
            .ThenInclude(orderItem => orderItem.Product)
            .ThenInclude(product => product.Seller)
            .ThenInclude(user => user.SellerInfo)
            .Where(o => o.CustomerId == userId && o.Status != OrderStatus.Pending)
            .OrderByDescending(o => o.CreatedAt)
            .Select(o => new OrderDto
            {
                Id = o.Id,
                Products = o.OrderItems.Select(oi => new OrderItemDto
                {
                    Id = oi.Id,
                    Product = new OrderItemProductDto
                    {
                        Id = oi.Product.Id,
                        Name = oi.Product.Name,
                        Description = oi.Product.Description,
                        Price = oi.Product.Stocks.SingleOrDefault(s => s.SizeId == oi.SizeId && s.ColorId == oi.ColorId).Price,
                        ImageUrl = oi.Product.Images.SingleOrDefault(i => oi.ColorId == i.ColorId).ImageUrls.FirstOrDefault(),
                        Color = new ColorDto
                        {
                            Id = oi.Color.Id,
                            Name = oi.Color.Name,
                            HexCode = oi.Color.HexCode,
                        },
                        Size = new SizeDto
                        {
                            Id = oi.Size.Id,
                            Name = oi.Size.Name,
                            Value = oi.Size.Value
                        },
                        SellerName = oi.Product.Seller.SellerInfo.Name ?? "Unknown",
                        SellerId = oi.Product.SellerId
                    },
                    Quantity = oi.Quantity
                }).ToList(),
                TotalPrice = o.OrderItems.Sum(oi => oi.Product.Stocks.SingleOrDefault(s => s.SizeId == oi.SizeId && s.ColorId == oi.ColorId).Price * oi.Quantity),
                TotalQuantity = o.OrderItems.Sum(oi => oi.Quantity),
            })
            .ToListAsync();
        
        return orders;
    }


    private async Task<User> GetAuthorizedUserAsync(string userId)
    {
        var user = await _db.Users
            .AsNoTracking()
            .Include(u => u.BannedUser)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            throw new ArgumentException("User not found");

        if (user.BannedUser != null && user.BannedUser.BanEndTime > DateTime.Now)
            throw new UnauthorizedAccessException("User is banned");

        return user;
    }

    private async Task CheckIfUserIsSeller(User user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        if (!roles.Contains(UserRoles.Seller)) throw new UnauthorizedAccessException("User is not a seller");
    }
}

using ECommerce.Config;
using ECommerce.Models.DTOs.Cart;
using ECommerce.Models.DTOs.Color;
using ECommerce.Models.DTOs.Product;
using ECommerce.Models.DTOs.Review;
using ECommerce.Models.DTOs.Size;
using ECommerce.Models.DTOs.User;
using ECommerce.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CartDto = ECommerce.Models.DTOs.Cart.CartDto;

namespace ECommerce.Data.Repositories.Customer;

public class CustomerRepository : BaseRepository, ICustomerRepository
{
    private readonly UserManager<User> _userManager;

    public CustomerRepository(ProductDbContext context, UserManager<User> userManager) : base(context)
    {
        _userManager = userManager;
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

    public async Task<CartDto> GetCartForUser(string userId)
    {
        var userQuery = _db.Users
            .AsNoTracking()
            .Where(u => u.Id == userId);

        var cartQuery = userQuery
            .Select(u => u.Cart);

        var productsQuery = cartQuery
            .SelectMany(c => c.Products)
            .Where(i => i.Product != null && i.Color != null && i.Size != null);

        var cartDtoQuery = userQuery
            .Select(u => new CartDto
            {
                Id = u.Cart.Id,
                Customer = new UserDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    Username = u.FirstName,
                    ImageUrl = u.ImageUrl,
                    CreatedAt = u.CreatedAt,
                    UpdatedAt = u.UpdatedAt
                },
                Products = productsQuery.Select(i => new CartItemDto
                {
                    Id = i.Id,
                    Product = new CartItemProductDto
                    {
                        Id = i.Product.Id,
                        Name = i.Product.Name,
                        Description = i.Product.Description,
                        Price = i.Product.Stocks.OrderBy(s => s.Price).First().Price,
                        Color = new ColorDto
                        {
                            Id = i.ColorId,
                            HexCode = i.Color.HexCode,
                            Name = i.Color.Name
                        },
                        Size = new SizeDto
                        {
                            Id = i.SizeId,
                            Name = i.Size.Name,
                            Value = i.Size.Value
                        },
                        ImageUrl = i.Product.Images.FirstOrDefault(productImage => productImage.ColorId == i.ColorId)
                            .ImageUrls.FirstOrDefault(),
                        SellerId = i.Product.SellerId,
                        SellerName = i.Product.Seller.FirstName
                    },
                    Quantity = i.Quantity
                }).ToList()
            });

        var cart = await cartDtoQuery.FirstOrDefaultAsync() ?? new CartDto
        {
            Customer = new UserDto
            {
                Id = userId
            },
            Products = new List<CartItemDto>()
        };

        return cart;
    }

    public async Task AddProductToCart(string userId, CreateCartItemDto cartProduct)
    {
        var user = await _db.Users
            .Include(u => u.Cart)
            .ThenInclude(c => c.Products)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null) throw new ArgumentException("User not found");

        var product = await _db.Products
            .AsNoTracking()
            .Include(p => p.Stocks)
            .FirstOrDefaultAsync(p => p.Id == cartProduct.ProductId);
        if (product == null) throw new ArgumentException("Product not found");

        var stock = product
            .Stocks
            .FirstOrDefault(s => s.ColorId == cartProduct.ColorId && s.SizeId == cartProduct.SizeId);

        if (stock == null) throw new ArgumentException("Stock not found");
        if (stock.Stock < cartProduct.Quantity) throw new ArgumentException("Not enough stock for this product");

        var color = await _db.Colors.FindAsync(cartProduct.ColorId);
        if (color == null) throw new ArgumentException("Color not found");

        var size = await _db.Sizes.FindAsync(cartProduct.SizeId);
        if (size == null) throw new ArgumentException("Size not found");

        if (user.Cart == null)
        {
            user.Cart = new Cart
            {
                Id = new Guid(),
                CustomerId = userId
            };
            _db.Carts.Add(user.Cart);
        }

        var existingProduct = user.Cart.Products
            .FirstOrDefault(p => p.ProductId == cartProduct.ProductId
                                 && p.ColorId == cartProduct.ColorId
                                 && p.SizeId == cartProduct.SizeId);

        if (existingProduct != null)
            existingProduct.Quantity += cartProduct.Quantity;
        else
            await _db.CartProducts.AddAsync(new CartProduct
            {
                CartId = user.Cart.Id,
                ProductId = cartProduct.ProductId,
                ColorId = cartProduct.ColorId,
                SizeId = cartProduct.SizeId,
                Quantity = cartProduct.Quantity
            });

        await SaveChangesAsyncWithTransaction();
    }

    public async Task RemoveProductFromCart(string userId, Guid cartProductId)
    {
        var user = await _db.Users
            .Include(u => u.Cart)
            .ThenInclude(c => c.Products)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null) throw new ArgumentException("User not found");

        var product = user.Cart.Products.FirstOrDefault(p => p.Id == cartProductId);
        if (product == null) throw new ArgumentException("Product not found in cart");

        _db.CartProducts.Remove(product);

        await SaveChangesAsyncWithTransaction();
    }

    public async Task UpdateProductInCart(string userId, Guid cartProductId, ChangeCartItemDto cartProduct)
    {
        var cart = await _db.Carts
            .Where(c => c.CustomerId == userId)
            .Include(c => c.Products)
            .FirstOrDefaultAsync();
        
        if (cart == null) throw new ArgumentException("Cart not found");
        
        var product = cart.Products.FirstOrDefault(p => p.Id == cartProductId);
        
        if (product == null) throw new ArgumentException("Product not found in cart");
        
        product.Quantity = cartProduct.Quantity;
        
        await SaveChangesAsyncWithTransaction();
    }

    public async Task ClearCart(string userId)
    {
        var cart = await _db.Carts
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.CustomerId == userId);
        
        if (cart == null) throw new ArgumentException("User not found or cart is empty");
        
        _db.CartProducts.RemoveRange(cart.Products);
        
        await SaveChangesAsyncWithTransaction();
    }

    public Task Checkout(string userId)
    {
        // TODO: Implement after order system is implemented
        throw new NotImplementedException();
    }

    private async Task<User> GetAuthorizedUserAsync(string userId)
    {
        var user = await _db.Users
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
using ECommerce.Config;
using ECommerce.Models.DTOs.Color;
using ECommerce.Models.DTOs.Order;
using ECommerce.Models.DTOs.Size;
using ECommerce.Models.DTOs.User;
using ECommerce.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using OrderItemStatus = ECommerce.Models.Enum.OrderItemStatus;

namespace ECommerce.Data.Repositories.Seller;

public class SellerRepository : BaseRepository, ISellerRepository
{
    private readonly UserManager<User> _userManager;

    public SellerRepository(ProductDbContext db, UserManager<User> userManager) : base(db)
    {
        _userManager = userManager;
    }

    public async Task UpdateSellerInfo(string id, SellerInfo sellerInfo)
    {
        var user = await _userManager.FindByIdAsync(id) ?? throw new ArgumentException("User not found");
        user.SellerInfo = sellerInfo;

        await SaveChangesAsyncWithTransaction();
    }

    public async Task<SellerInfoDto> GetSellerInfo(string id)
    {
        var user = await _db
            .Users
            .AsNoTracking()
            .Where(u => u.Id == id && u.SellerInfoId != null)
            .Select(u => new SellerInfoDto
            {
                Id = u.SellerInfo.Id,
                Name = u.SellerInfo.Name,
                Description = u.SellerInfo.Description,
                Address = u.SellerInfo.Address,
                Phone = u.SellerInfo.Phone,
                Email = u.SellerInfo.Email,
                Website = u.SellerInfo.Website,
                Logo = u.SellerInfo.Logo,
                Banner = u.SellerInfo.Banner,
                Facebook = u.SellerInfo.Facebook,
                Twitter = u.SellerInfo.Twitter,
                Instagram = u.SellerInfo.Instagram,
                Linkedin = u.SellerInfo.Linkedin,
                Youtube = u.SellerInfo.Youtube,
                Tiktok = u.SellerInfo.Tiktok,
                Snapchat = u.SellerInfo.Snapchat,
                Pinterest = u.SellerInfo.Pinterest,
                Reddit = u.SellerInfo.Reddit,
                Tumblr = u.SellerInfo.Tumblr,
                Whatsapp = u.SellerInfo.Whatsapp,
                Telegram = u.SellerInfo.Telegram,
                Signal = u.SellerInfo.Signal,
                Viber = u.SellerInfo.Viber,
                Wechat = u.SellerInfo.Wechat,
                Line = u.SellerInfo.Line,
                Weibo = u.SellerInfo.Weibo,
                Vk = u.SellerInfo.Vk,
                Skype = u.SellerInfo.Skype,
                Discord = u.SellerInfo.Discord,
                Twitch = u.SellerInfo.Twitch,
                Clubhouse = u.SellerInfo.Clubhouse,
                Patreon = u.SellerInfo.Patreon,
                Cashapp = u.SellerInfo.Cashapp,
                Paypal = u.SellerInfo.Paypal,
                Venmo = u.SellerInfo.Venmo,
                Zelle = u.SellerInfo.Zelle,
                Applepay = u.SellerInfo.Applepay,
                Googlepay = u.SellerInfo.Googlepay,
                Samsungpay = u.SellerInfo.Samsungpay,
                OrderCount = u.Orders.Count(),
                ProductCount = u.Stores.Sum(s => s.Collections.Sum(c => c.Products.Count)),
                CollectionCount = u.Stores.Sum(s => s.Collections.Count),
                ReviewCount = u.Reviews.Count(),
                IsVerified = u.SellerInfo.IsVerified,
                CreatedAt = u.SellerInfo.CreatedAt,
                UpdatedAt = u.SellerInfo.UpdatedAt ?? u.SellerInfo.CreatedAt
            })
            .SingleOrDefaultAsync();

        if (user == null) throw new ArgumentException("User not found or not a seller");

        return user;
    }
    
    public async Task<IEnumerable<OrderItemDto>> GetOrdersForSeller(string id)
    {
        return await _db
            .OrderItems
            .AsNoTracking()
            .Select(oi => new OrderItemDto
            {
                Id = oi.Id,
                Status = oi.Status.GetDisplayName(),
                Product = new OrderItemProductDto
                {
                    Id = oi.Product.Id,
                    Name = oi.Product.Name,
                    Description = oi.Product.Description,
                    Price = oi.Product.Stocks.SingleOrDefault(s => s.SizeId == oi.SizeId && s.ColorId == oi.ColorId)
                        .Price,
                    ImageUrl = oi.Product.Images.SingleOrDefault(i => oi.ColorId == i.ColorId).ImageUrls
                        .FirstOrDefault(),
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
                Customer = new UserDto
                {
                    Id = oi.Order.Customer.Id,
                    Username = oi.Order.Customer.UserName,
                    Email = oi.Order.Customer.Email,
                    PhoneNumber = oi.Order.Customer.PhoneNumber,
                    ImageUrl = oi.Order.Customer.ImageUrl,
                    Role = UserRoles.Customer,
                },
                Quantity = oi.Quantity
            }).ToListAsync();
    }
    
    public async Task UpdateOrderStatus(Guid id, string userId, OrderItemStatus status, bool isAdmin = false)
    {
        var orderItem = await _db.OrderItems
                            .Include(oi => oi.Product)
                            .SingleOrDefaultAsync(oi => oi.Id == id) ??
                        throw new ArgumentException("Order item not found");
        if (orderItem.Product.SellerId != userId && !isAdmin)
            throw new ArgumentException("You are not the seller of this order's product");
        if (orderItem.Status is OrderItemStatus.Cancelled or OrderItemStatus.Delivered)
            throw new ArgumentException("Order item status cannot be updated");
        
        orderItem.Status = status;

        await SaveChangesAsyncWithTransaction();
    }
}
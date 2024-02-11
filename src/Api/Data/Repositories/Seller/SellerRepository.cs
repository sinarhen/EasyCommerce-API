using ECommerce.Models.DTOs.User;
using ECommerce.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Data.Repositories.Seller;

public class SellerRepository: BaseRepository, ISellerRepository
{
    private readonly UserManager<User> _userManager;
    protected SellerRepository(ProductDbContext db, UserManager<User> userManager) : base(db)
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
            Rating = u.Reviews.Average(r => (double)r.Rating),
            IsVerified = u.SellerInfo.IsVerified,
            CreatedAt = u.SellerInfo.CreatedAt,
            UpdatedAt = u.SellerInfo.UpdatedAt ?? u.SellerInfo.CreatedAt
        })
        .FirstOrDefaultAsync();

    if (user == null)
    {
        throw new ArgumentException("User not found");
    }

    return user;
}
}
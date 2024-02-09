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
}
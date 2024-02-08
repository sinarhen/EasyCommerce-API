using ECommerce.Config;
using ECommerce.Data;
using ECommerce.Data.Repositories;
using ECommerce.Models.DTOs.Product;
using ECommerce.Models.DTOs.Review;
using ECommerce.Models.DTOs.User;
using ECommerce.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Customer;

public class CustomerRepository : BaseRepository, ICustomerRepository
{
    private readonly UserManager<User> _userManager;
    public CustomerRepository(ProductDbContext context, UserManager<User> userManager) : base(context)
    {
        _userManager = userManager;
    }


    public async Task<UserReviewsDto> GetReviewsForUser(string userId)
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

        var user = await _userManager.FindByIdAsync(userId);
        var roles = await _userManager.GetRolesAsync(user);

        var isBanned = await _db.BannedUsers.AsNoTracking().AnyAsync(b => b.UserId == user.Id);
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
                IsBanned = isBanned
            },
            Reviews = reviews
        }; 
    }

    public async Task<bool> RequestUpgradingToSeller(string userId, SellerInfoDto sellerInfo)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ArgumentException("User id is required");
        }
        if (sellerInfo == null)
        {
            throw new ArgumentException("Empty body");
        }
        if (sellerInfo.Name == null)
        {
            throw new ArgumentException("Company name is required");
        }
        if (sellerInfo.Description == null)
        {
            throw new ArgumentException("Company description is required");
        }
        if (sellerInfo.Email == null)
        {
            throw new ArgumentException("Company email is required");
        }
        if (sellerInfo.PhoneNumber == null)
        {
            throw new ArgumentException("Company phone number is required");
        }
        

        await CheckIfUserIsSeller(userId);
        var seller = new SellerInfo
        {
            CompanyName = sellerInfo.Name,
            CompanyDescription = sellerInfo.Description,
            CompanyEmail = sellerInfo.Email,
            CompanyPhone = sellerInfo.PhoneNumber,
        };
        await _db.Sellers.AddAsync(seller);
        await _db.SellerUpgradeRequests.AddAsync(new SellerUpgradeRequest
        {
            UserId = userId,
            SellerInfoId = seller.Id
        });
        await SaveChangesAsyncWithTransaction();
        return true;
    }
    private async Task CheckIfUserIsSeller(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId) ?? throw new ArgumentException("User not found");
        var roles = await _userManager.GetRolesAsync(user);
        if (roles.Contains(UserRoles.Seller))
        {
            throw new ArgumentException("User is already a seller");
        }



    }
}


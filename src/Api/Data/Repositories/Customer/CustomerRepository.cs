using ECommerce.Data;
using ECommerce.Data.Repositories;
using ECommerce.Models.DTOs.Product;
using ECommerce.Models.DTOs.Review;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Customer;

public class CustomerRepository : BaseRepository, ICustomerRepository
{
    public CustomerRepository(ProductDbContext context) : base(context)
    {
    }


    public async Task<List<UserReviewsDto>> GetReviewsForUser(string userId)
    {
        var reviews = await _db.Reviews
                .Where(r => r.CustomerId == userId)
                .Select(r => new ReviewDto
                {
                    Id = r.Id,
                    UserId = r.CustomerId,
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

        var user = await 
    }
}

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


    public async Task<List<ReviewDto>> GetReviewsForUser(string userId)
    {
        return 
            await _db.Reviews
                .Where(r => r.CustomerId == userId)
                .Select(r => new ReviewDto
                {
                    Id = r.Id,
                    Rating = r.Rating,
                    Content = r.Content,
                    CustomerName = r.User.UserName,
                    Product = new ReviewProductDto
                    {
                        Id = r.Product.Id,
                        Name = r.Product.Name,
                        Description = r.Product.Description,
                        ImageUrl = r.Product.Images.FirstOrDefault().ImageUrls.FirstOrDefault(),
                    }
                })
                .ToListAsync();
    }
}

using ECommerce.Data;
using ECommerce.Data.Repositories;
using ECommerce.Models.DTOs.Review;
using ECommerce.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Review;
public class ReviewRepository : BaseRepository, IReviewRepository
{
    public ReviewRepository(ProductDbContext context) : base(context)
    {
    }

    public async Task<ECommerce.Models.Entities.Review> CreateReviewForProduct(Guid productId, string userId, CreateReviewDto createReviewDto)
    {
        var userExists = await _db.Users.AnyAsync(u => u.Id == userId);
        if (!userExists)
        {
            throw new ArgumentException("User not found");
        }
        var productExists = await _db.Products.AnyAsync(p => p.Id == productId);
        if (!productExists)
        {
            throw new ArgumentException("Product not found");
        }

        var review = new ECommerce.Models.Entities.Review
        {
            ProductId = productId,
            CustomerId = userId,
            Rating = createReviewDto.Rating,
            Content = createReviewDto.Content
        };

        await _db.Reviews.AddAsync(review);
        await SaveChangesAsyncWithTransaction();

        return review;
    }

    public Task DeleteReviewForCollectionAsync(Guid reviewId, string userId)
    {
        throw new NotImplementedException();
    }
}
using ECommerce.Entities.Enum;
using ECommerce.Models.DTOs.Review;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Data.Repositories.Review;

public class ReviewRepository : BaseRepository, IReviewRepository
{
    public ReviewRepository(ProductDbContext context) : base(context)
    {
    }

    public async Task<ECommerce.Models.Entities.Review> CreateReviewForProduct(Guid productId, string userId,
        CreateReviewDto createReviewDto)
    {
        var userExists = await _db.Users.AnyAsync(u => u.Id == userId);
        if (!userExists) throw new ArgumentException("User not found");
        var productExists = await _db.Products.AnyAsync(p => p.Id == productId);
        if (!productExists) throw new ArgumentException("Product not found");
        if (Enum.IsDefined(typeof(Rating), createReviewDto.Rating) == false)
            throw new ArgumentException("Invalid rating. Must be integer number between 1 and 5");


        var review = new ECommerce.Models.Entities.Review
        {
            ProductId = productId,
            CustomerId = userId,
            Title = createReviewDto.Title,
            Rating = createReviewDto.Rating,
            Content = createReviewDto.Content
        };

        await _db.Reviews.AddAsync(review);
        await SaveChangesAsyncWithTransaction();

        return review;
    }

    public async Task DeleteReviewForCollectionAsync(Guid reviewId, string userId)
    {
        if (!await _db.Users.AnyAsync(u => u.Id == userId)) throw new ArgumentException("User not found");

        var review = await _db.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId)
                     ?? throw new ArgumentException("Review not found");

        if (review.CustomerId != userId)
            throw new UnauthorizedAccessException("You are not allowed to delete this review");

        _db.Reviews.Remove(review);
        await SaveChangesAsyncWithTransaction();
    }
}
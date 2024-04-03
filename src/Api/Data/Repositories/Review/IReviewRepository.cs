using ECommerce.Models.DTOs.Review;

namespace ECommerce.Data.Repositories.Review;

public interface IReviewRepository
{
    Task<Models.Entities.Review> CreateReviewForProduct(Guid productId, string userId,
        CreateReviewDto createReviewDto); // TODO: [FromBody] CreateReviewDto createReviewDto

    Task DeleteReviewForProductAsync(Guid reviewId, string userId);
}
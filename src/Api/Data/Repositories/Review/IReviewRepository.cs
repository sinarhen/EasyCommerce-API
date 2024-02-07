using ECommerce.Models.DTOs.Review;

namespace Data.Repositories.Review;

public interface IReviewRepository
{
    Task<ECommerce.Models.Entities.Review> CreateReviewForProduct(Guid productId, string userId, CreateReviewDto createReviewDto); // TODO: [FromBody] CreateReviewDto createReviewDto
    Task DeleteReviewForCollectionAsync(Guid reviewId, string userId);
}
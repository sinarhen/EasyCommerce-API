namespace Data.Repositories.Review;

public interface IReviewRepository
{
    Task<ECommerce.Models.Entities.Review> CreateReviewForProduct(Guid productId); // TODO: [FromBody] CreateReviewDto createReviewDto
    Task DeleteReviewForCollectionAsync(Guid reviewId);
}
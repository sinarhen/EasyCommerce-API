using ECommerce.Data;
using ECommerce.Data.Repositories;
using ECommerce.Models.DTOs.Review;

namespace Data.Repositories.Review;
public class ReviewRepository : BaseRepository, IReviewRepository
{
    public ReviewRepository(ProductDbContext context) : base(context)
    {
    }

    public Task<ECommerce.Models.Entities.Review> CreateReviewForProduct(Guid productId, string userId, CreateReviewDto createReviewDto)
    {
        throw new NotImplementedException();
    }

    public Task DeleteReviewForCollectionAsync(Guid reviewId, string userId)
    {
        throw new NotImplementedException();
    }
}
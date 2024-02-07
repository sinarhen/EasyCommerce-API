using ECommerce.Data;
using ECommerce.Data.Repositories;

namespace Data.Repositories.Review;
public class ReviewRepository : BaseRepository, IReviewRepository
{
    public ReviewRepository(ProductDbContext context) : base(context)
    {
    }

    public Task<ECommerce.Models.Entities.Review> CreateReviewForProduct(Guid productId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteReviewForCollectionAsync(Guid reviewId)
    {
        throw new NotImplementedException();
    }
}
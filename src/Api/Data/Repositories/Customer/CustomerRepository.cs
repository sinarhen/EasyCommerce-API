using ECommerce.Data;
using ECommerce.Data.Repositories;

namespace Data.Repositories.Customer;

public class CustomerRepository : BaseRepository, ICustomerRepository
{
    public CustomerRepository(ProductDbContext context) : base(context)
    {
    }
    Task<List<ECommerce.Models.Entities.Review>> ICustomerRepository.GetReviewsForUser(string userId)
    {
        throw new NotImplementedException();
    }
}

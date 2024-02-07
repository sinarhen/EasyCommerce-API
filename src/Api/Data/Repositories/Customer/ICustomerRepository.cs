using ECommerce.Models.Entities;

namespace Data.Repositories.Customer;

public interface ICustomerRepository
{
    Task<List<ECommerce.Models.Entities.Review>> GetReviewsForUser(string userId);


}
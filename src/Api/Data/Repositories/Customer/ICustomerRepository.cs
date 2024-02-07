using ECommerce.Models.DTOs.Product;
using ECommerce.Models.Entities;

namespace Data.Repositories.Customer;

public interface ICustomerRepository
{
    Task<List<ReviewDto>> GetReviewsForUser(string userId);


}
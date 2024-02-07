using ECommerce.Models.DTOs.Product;

namespace Data.Repositories.Customer;

public interface ICustomerRepository
{
    Task<UserReviewsDto> GetReviewsForUser(string userId);


}
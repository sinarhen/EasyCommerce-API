using ECommerce.Models.DTOs.Product;
using ECommerce.Models.DTOs.User;

namespace Data.Repositories.Customer;

public interface ICustomerRepository
{
    Task<UserReviewsDto> GetReviewsForUser(string userId);

    Task<bool> UpgradeToSeller(string userId, SellerInfoDto sellerInfo);

}
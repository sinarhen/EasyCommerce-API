using ECommerce.Models.DTOs.Cart;
using ECommerce.Models.DTOs.Order;
using ECommerce.Models.DTOs.Product;
using ECommerce.Models.DTOs.User;

namespace ECommerce.Data.Repositories.Customer;

public interface ICustomerRepository
{
    Task<UserReviewsDto> GetReviewsForUser(string userId, IReadOnlyList<string> roles);

    Task RequestUpgradingToSeller(string userId, SellerInfoCreateDto sellerInfo);

    Task<OrderDto> GetCartForUser(string userId);
    
    Task<List<OrderDto>> GetOrdersForUser(string userId);
 
    Task AddProductToCart(string userId, CreateCartItemDto cartProduct);

    Task RemoveProductFromCart(string userId, Guid cartProductId);

    Task UpdateProductInCart(string userId, Guid cartProductId, ChangeCartItemDto cartProduct);

    Task ConfirmCart(string userId);

    Task ClearCart(string userId);

}
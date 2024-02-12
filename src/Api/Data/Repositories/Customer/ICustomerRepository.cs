using ECommerce.Models.DTOs.Product;
using ECommerce.Models.DTOs.User;
using ECommerce.Models.Entities;

namespace ECommerce.Data.Repositories.Customer;

public interface ICustomerRepository
{
    Task<UserReviewsDto> GetReviewsForUser(string userId);

    Task<bool> RequestUpgradingToSeller(string userId, SellerInfoCreateDto sellerInfo);
    
    Task<Cart> GetCartForUser(string userId);
    
    Task<bool> AddProductToCart(string userId, CartProduct cartProduct); // TODO: CreateCartProductDto
    
    Task<bool> RemoveProductFromCart(string userId, Guid cartProductId);
    
    Task<bool> UpdateProductInCart(string userId, CartProduct cartProduct); // TODO: CreateCartProductDto
    
    Task<bool> ClearCart(string userId);
    
    Task<bool> Checkout(string userId); 
}
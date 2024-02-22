using ECommerce.Models.DTOs.Order;
using ECommerce.Models.DTOs.User;
using ECommerce.Models.Entities;
using OrderItemStatus = ECommerce.Models.Enum.OrderItemStatus;

namespace ECommerce.Data.Repositories.Seller;

public interface ISellerRepository
{
    // Task<SellerInfoDto> GetSellerInfo(string id);

    Task UpdateSellerInfo(string id, SellerInfo sellerInfo);
    Task<SellerInfoDto> GetSellerInfo(string id);
    
    Task<IEnumerable<OrderItemDto>> GetOrdersForSeller(string id);
    
    Task UpdateOrderStatus(Guid id, string userId, OrderItemStatus status, bool isAdmin = false);
}
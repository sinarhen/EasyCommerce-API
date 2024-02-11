using ECommerce.Models.DTOs.User;
using ECommerce.Models.Entities;

namespace ECommerce.Data.Repositories.Seller;

public interface ISellerRepository
{
    // Task<SellerInfoDto> GetSellerInfo(string id);
    
    Task UpdateSellerInfo(string id, SellerInfo sellerInfo);
    Task<SellerInfoDto> GetSellerInfo(string id);


}
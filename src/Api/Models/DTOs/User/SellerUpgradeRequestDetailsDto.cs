using ECommerce.Models.Entities;

namespace ECommerce.Models.DTOs.User;

public class SellerUpgradeRequestDetailsDto : SellerUpgradeRequestDto
{
    public SellerInfo SellerInfo { get; set; }
}
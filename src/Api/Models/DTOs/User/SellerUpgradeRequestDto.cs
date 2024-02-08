namespace ECommerce.Models.DTOs.User;
public class SellerUpgradeRequestDto
{
    public Guid Id { get; set; }
    public string Message { get; set; }
    public Guid SellerInfoId { get; set; }
}
using ECommerce.Models.Entities;

namespace ECommerce.Models.DTOs.User;

public class SellerUpgradeRequestDto
{
    public Guid Id { get; set; }
    public string Message { get; set; }
    public string Status { get; set; }
    public DateTime? DecidedAt { get; set; }
    public UserDto User { get; set; }
}
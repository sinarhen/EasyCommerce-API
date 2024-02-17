using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.DTOs.User;

public class SellerUpgradeRequestDto
{
    public Guid Id { get; set; }
    public string Message { get; set; }


    [Required] public string Status { get; set; }

    public DateTime? DecidedAt { get; set; }
    public UserDto User { get; set; }
    public DateTime CreatedAt { get; set; }
}
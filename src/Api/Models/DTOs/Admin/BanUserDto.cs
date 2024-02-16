using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.DTOs.Admin;

public class BanUserDto
{
    
    [Required]
    [StringLength(200, MinimumLength = 6, ErrorMessage = "Reason should be between 6 and 200 characters.")]
    public string Reason { get; set; }
    public DateTime? BanEndTime { get; set; }
}
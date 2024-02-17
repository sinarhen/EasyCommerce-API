using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.DTOs.Auth;

public class ChangePasswordDto
{
    [Required] [EmailAddress] public string Email { get; set; }

    [StringLength(55, MinimumLength = 6, ErrorMessage = "Password should be between 6 and 55 characters.")]
    public string OldPassword { get; set; }

    [Required]
    [StringLength(55, MinimumLength = 6, ErrorMessage = "Password should be between 6 and 55 characters.")]
    public string NewPassword { get; set; }
}
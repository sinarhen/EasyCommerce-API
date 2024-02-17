using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.DTOs.Auth;

public class ChangeEmailDto
{
    [Required] [EmailAddress] public string OldEmail { get; set; }

    [StringLength(55, MinimumLength = 6, ErrorMessage = "Password should be between 6 and 55 characters.")]
    public string Password { get; set; }

    [Required] [EmailAddress] public string NewEmail { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.DTOs.Auth;

public class RegisterDto
{
    [Required] public string UserName { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    [EmailAddress]
    [Required] 
    public string Email { get; set; }

    [Required] 
    [StringLength(55, MinimumLength = 6, ErrorMessage = "Password should be between 6 and 55 characters.")]
    public string Password { get; set; }
}
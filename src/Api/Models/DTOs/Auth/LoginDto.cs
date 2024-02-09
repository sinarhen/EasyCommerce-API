using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.DTOs.Auth;

public class LoginDto
{
    [Required] public string Email { get; set; }

    [Required] public string Password { get; set; }
}
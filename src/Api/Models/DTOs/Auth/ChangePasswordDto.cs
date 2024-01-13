namespace ECommerce.Models.DTOs.Auth;

public class ChangePasswordDto
{
    public string Email { get; set; }
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}
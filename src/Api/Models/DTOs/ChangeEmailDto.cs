namespace ECommerce.Models.DTOs;

public class ChangeEmailDto
{
    public string OldEmail { get; set; }
    public string Password { get; set; }
    public string NewEmail { get; set; }
}
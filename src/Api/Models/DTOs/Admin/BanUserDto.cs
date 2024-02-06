namespace ECommerce.Models.DTOs.Admin;

public class BanUserDto
{
    public string UserId { get; set; }
    public string Reason { get; set; }
    public DateTime? BanEndTime { get; set; }
}
namespace ECommerce.Models.DTOs.Admin;

public class BanUserDto
{
    public string Reason { get; set; }
    public DateTime? BanEndTime { get; set; }
}
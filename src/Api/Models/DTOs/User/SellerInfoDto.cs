namespace ECommerce.Models.DTOs.User;

public class SellerInfoCreateDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    
    public bool IsVerified { get; set; }

}
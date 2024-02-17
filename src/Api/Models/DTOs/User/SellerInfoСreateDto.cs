using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.DTOs.User;

public class SellerInfoCreateDto
{
    [Required]
    [StringLength(100, ErrorMessage = "Name is too long. max 100 characters.")]
    public string Name { get; set; }

    [StringLength(3000, ErrorMessage = "Description is too long. max 3000 characters.")]
    public string Description { get; set; }

    [StringLength(200, ErrorMessage = "Address is too long. max 200 characters.")]
    public string Address { get; set; }

    [StringLength(20, ErrorMessage = "Phone number is too long. max 20 characters.")]
    public string Phone { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(100, ErrorMessage = "Email is too long. max 100 characters.")]
    public string Email { get; set; }

    public bool IsVerified { get; set; }
}
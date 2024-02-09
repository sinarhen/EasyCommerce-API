using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Models.Entities;

public class User : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string PostalCode { get; set; }
    public string ImageUrl { get; set; }
    public Guid? CartId { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Guid? SellerInfoId { get; set; }
    
    [Required]
    [ForeignKey("SellerInfoId")]
    public SellerInfo SellerInfo { get; set; }    
    // Navigation properties
    [Required]
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    [Required]
    public ICollection<Store> Stores { get; set; } = new List<Store>();
    [Required]
    [ForeignKey("CartId")]
    public Cart Cart { get; set; }
}
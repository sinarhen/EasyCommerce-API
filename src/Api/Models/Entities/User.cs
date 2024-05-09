using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Models.Entities;

public class User : IdentityUser
{
    [StringLength(100)] public string FirstName { get; set; }

    [StringLength(100)] public string LastName { get; set; }

    [StringLength(100)] public string Address { get; set; }

    [StringLength(100)] public string City { get; set; }

    [StringLength(100)] public string Country { get; set; }

    [StringLength(10)] public string PostalCode { get; set; }

    [StringLength(2000)]
    [Url(ErrorMessage = "Invalid URL")]
    public string ImageUrl { get; set; }

    public Guid? CartId { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Guid? SellerInfoId { get; set; }

    // Navigation properties
    [ForeignKey("SellerInfoId")] public SellerInfo SellerInfo { get; set; }

    public BannedUser BannedUser { get; set; }

    public ICollection<SellerUpgradeRequest> Requests { get; set; } = new List<SellerUpgradeRequest>();

    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();

    public ICollection<Store> Stores { get; set; } = new List<Store>();
    
    // public ICollection<IdentityRole>
}
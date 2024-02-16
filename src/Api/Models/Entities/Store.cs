using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models.Entities;

public class Store : BaseEntity
{
    public Guid Id { get; set; }
    
    [StringLength(55, ErrorMessage = "Name is too long. max 55 characters.")]
    public string Name { get; set; }
    
    [StringLength(3000, ErrorMessage = "Description is too long. max 3000 characters.")]
    public string Description { get; set; }
    
    [StringLength(2000, ErrorMessage = "Banner URL is too long")]
    public string BannerUrl { get; set; }
    
    [StringLength(2000, ErrorMessage = "Logo URL is too long")]
    [Url]
    public string LogoUrl { get; set; }
    
    [StringLength(100, ErrorMessage = "Address is too long")]
    public string Address { get; set; }
    
    [StringLength(100, ErrorMessage = "City is too long")]
    public string Contacts { get; set; }
    
    [EmailAddress]
    [StringLength(100, ErrorMessage = "Email is too long")]
    public string Email { get; set; }

    public string OwnerId { get; set; }

    public bool IsVerified { get; set; }


    // Navigation properties
    [ForeignKey("OwnerId")] public User Owner { get; set; }

    [Required] public List<Collection> Collections { get; set; }
}
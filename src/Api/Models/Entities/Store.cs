using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models.Entities;

public class Store : BaseEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string BannerUrl { get; set; }
    public string LogoUrl { get; set; }
    public string Address { get; set; }
    public string Contacts { get; set; }
    public string Email { get; set; }
    
    public string OwnerId { get; set; }
    
    public bool IsVerified { get; set; }
    
    
    // Navigation properties
    [ForeignKey("OwnerId")]
    public User Owner { get; set; }
    public List<Collection> Collections { get; set; }
    
    
}
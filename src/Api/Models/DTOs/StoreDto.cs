using ECommerce.Models.Entities;

namespace ECommerce.Models.DTOs;

public class StoreDto : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string BannerUrl { get; set; }
    public string LogoUrl { get; set; }
    public string Address { get; set; }
    public string Contacts { get; set; }
    public string Email { get; set; }
    
}


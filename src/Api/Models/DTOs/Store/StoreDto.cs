using ECommerce.Models.Entities;

namespace ECommerce.Models.DTOs.Store;

public class StoreDto : BaseEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string BannerUrl { get; set; }
    public string LogoUrl { get; set; }
    public string Address { get; set; }
    public string Contacts { get; set; }
    public string Email { get; set; }
    public bool IsVerified { get; set; }
    public string Owner { get; set; } // TODO: Create User Dto
}
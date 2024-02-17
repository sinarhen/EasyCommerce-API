using System.ComponentModel.DataAnnotations;
using ECommerce.Models.DTOs.User;
using ECommerce.Models.Entities;

namespace ECommerce.Models.DTOs.Store;

public class StoreDto : BaseEntity
{
    public Guid Id { get; set; }

    [Required]
    [StringLength(55, MinimumLength = 3, ErrorMessage = "Name should be between 3 and 55 characters.")]
    public string Name { get; set; }

    [StringLength(3000, ErrorMessage = "Description is too long. max 3000 characters.")]
    public string Description { get; set; }

    [StringLength(2000, ErrorMessage = "Banner URL is too long")]
    [Url]
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

    public bool IsVerified { get; set; }
    public UserDto Owner { get; set; }
}
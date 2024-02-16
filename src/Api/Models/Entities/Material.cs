using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Models.Entities;

[PrimaryKey("Id")]
public class Material : BaseEntity
{
    [Key] public Guid Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Name should be between 6 and 100 characters.")]
    public string Name { get; set; }

    // Navigation properties
    public ICollection<ProductMaterial> Products { get; set; } = new List<ProductMaterial>();
}
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Models.Entities;

[PrimaryKey("Id")]
public class Material : BaseEntity
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }

    // Navigation properties
    public ICollection<ProductMaterial> Products { get; set; } = new List<ProductMaterial>();
}
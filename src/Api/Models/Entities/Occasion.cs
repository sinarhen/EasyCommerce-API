using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Models.Entities;

[PrimaryKey("Id")]
public class Occasion : BaseEntity
{
    [Key] public Guid Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Name should be between 6 and 100 characters.")]
    public string Name { get; set; }

    [StringLength(3000, ErrorMessage = "Description is too long. max 3000 characters.")]
    public string Description { get; set; }


    // Navigation properties
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
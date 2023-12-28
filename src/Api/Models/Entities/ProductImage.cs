using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Models.Entities;

[PrimaryKey("ProductId", "ColorId")]
public class ProductImage
{
    public Product Product { get; set; }

    [Key] public Guid ProductId { get; set; }

    public Color Color { get; set; }

    [Key] public Guid ColorId { get; set; }

    public List<string> ImagesUrl { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
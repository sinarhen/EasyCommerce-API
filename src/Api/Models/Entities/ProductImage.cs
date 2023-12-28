using Microsoft.EntityFrameworkCore;

namespace ECommerce.Models.Entities;

[PrimaryKey("ProductId", "ColorId")]
public class ProductImage
{
    public Product Product { get; set; }
    public Guid ProductId { get; set; }

    public Color Color { get; set; }
    public Guid ColorId { get; set; }
    
    public string Url { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
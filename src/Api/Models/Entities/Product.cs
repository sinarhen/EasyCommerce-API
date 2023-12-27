
namespace ECommerce.Models.Entities;

public class Product
{
    public Guid Id { get; set; }
    
    public Category Category { get; set; }
    public Guid CategoryId { get; set; }
    
    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public ICollection<ProductColorQuantity> ColorQuantities { get; set; } = new List<ProductColorQuantity>();

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
    
    public string ImageUrl { get; set; }
    
    public bool InStock { get; set; }
}

namespace ECommerce.Models.Entities;

public class Product
{
    public Guid Id { get; set; }
    
    public Category Category { get; set; }
    public Guid CategoryId { get; set; }
    
    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
    
    public List<string> ImagesUrl { get; set; } = new List<string>();
    
    // Navigation properties
    public ICollection<ProductStock> Stocks { get; set; } = new List<ProductStock>();
}
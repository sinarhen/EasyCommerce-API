
namespace ECommerce.Models.Entities;

public class Product
{
    public Guid Id { get; set; }
    
    public Category Category { get; set; }
    public Guid CategoryId { get; set; }
    
    public string Name { get; set; }

    public string Description { get; set; }

    public string Price { get; set; }

    public Color Color { get; set; }
    
    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
    
    public string ImageUrl { get; set; }
    
    public int InStock { get; set; }
}
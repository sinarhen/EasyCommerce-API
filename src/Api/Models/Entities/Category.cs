namespace ECommerce.Models.Entities;

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation properties
    public ICollection<Product> Products { get; set; } = new List<Product>();
    
}
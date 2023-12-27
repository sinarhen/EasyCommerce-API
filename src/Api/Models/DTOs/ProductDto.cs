namespace ECommerce.Models.DTOs;

public class ProductDto
{
    public Guid Id { get; set; }
    
    public string CategoryName { get; set; }
    
    public string Name { get; set; }

    public string Description { get; set; }

    public string Price { get; set; }

    
    public DateTime CreatedAt { get; set; }
    
    public string ImageUrl { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Dictionary<string, int> Quantities { get; set; } = new Dictionary<string, int>();
}
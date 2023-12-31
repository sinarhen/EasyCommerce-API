using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.Entities;

public class Size
{
    [Key]
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    public int Value { get; set; }

    
    
    // Navigation properties
    public ICollection<CategorySize> Categories { get; set; } = new List<CategorySize>();
    public ICollection<Product> Stocks { get; set; } = new List<Product>();
}

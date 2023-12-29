using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.Entities;

public class Size
{
    [Key]
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    // Navigation properties
    public ICollection<Product> Stocks { get; set; } = new List<Product>();
}
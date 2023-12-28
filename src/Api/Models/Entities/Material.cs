namespace ECommerce.Models.Entities;

public class Material
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    
    // Navigation properties
    public ICollection<ProductMaterials> Products { get; set; } = new List<ProductMaterials>();
}
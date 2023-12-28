namespace ECommerce.Models.Entities;

public class Material
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    // Navigation properties
    public ICollection<ProductMaterial> Products { get; set; } = new List<ProductMaterial>();
}
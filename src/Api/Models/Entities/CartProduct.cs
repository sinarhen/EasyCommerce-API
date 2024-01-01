namespace ECommerce.Models.Entities;

public class CartProduct : BaseEntity 
{
    public Guid Id { get; set; }
    public Guid CartId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public Guid SizeId { get; set; }
    public Guid ColorId { get; set; }
    
    // Navigation properties
    public Cart Cart { get; set; }
    public Product Product { get; set; }
    public Size Size { get; set; }
    public Color Color { get; set; }
    
    
}
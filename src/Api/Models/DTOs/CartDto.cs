using ECommerce.Models.DTOs.Product;

namespace ECommerce.Models.DTOs;

public class CartDto
{
    public Guid Id { get; set; }
    public ICollection<CartProductDto> Products { get; set; } = new List<CartProductDto>();
    public decimal TotalPrice { get; set; }
    public int TotalQuantity { get; set; }
    public string CustomerId { get; set; }
}


public class CartProductDto
{
    // TODO: Uncomment after implementing CartProductDto
    // public CartProductDto Product { get; set; }    
    public int Quantity { get; set; }
    public string CustomerId { get; set; }
}
namespace ECommerce.Models.DTOs;

public class CartDto
{
    public Guid Id { get; set; }
    // TODO: uncomment after implementing CartProductDto
    // public ICollection<CartProductDto> Products { get; set; } = new List<CartProductDto>();
    public decimal TotalPrice { get; set; }
    public int TotalQuantity { get; set; }
    public string CustomerId { get; set; }
}
using ECommerce.Models.DTOs.User;

namespace ECommerce.Models.DTOs.Cart;

public class CartDto
{
    public Guid Id { get; set; }
    public ICollection<CartItemDto> Products { get; set; } = new List<CartItemDto>();
    public decimal TotalPrice { get; set; }
    public int TotalQuantity { get; set; }
    public UserDto Customer { get; set; }
}
namespace ECommerce.Models.DTOs.Size;

public class SizeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Value { get; set; }

    public int? Quantity { get; set; }

    public bool? IsAvailable { get; set; }
}
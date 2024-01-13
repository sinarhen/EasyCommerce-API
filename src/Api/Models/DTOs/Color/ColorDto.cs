namespace ECommerce.Models.DTOs.Color;

public class ColorDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string HexCode { get; set; }
    public List<string> ImageUrls { get; set; }

    public bool? IsAvailable { get; set; }

    public int? Quantity { get; set; }
}
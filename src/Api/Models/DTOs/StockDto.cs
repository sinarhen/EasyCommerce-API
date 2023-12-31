namespace ECommerce.Models.DTOs;

public class StockDto
{
    public ColorDto Color { get; set; }
    public List<AvailabilityDto> Availability { get; set; }
    public List<string> ImageUrls { get; set; }
}
using ECommerce.Models.DTOs.Color;
using ECommerce.Models.DTOs.Product;

namespace ECommerce.Models.DTOs.Stock;
public class StockDto
{
    public ColorDto Color { get; set; }
    public List<AvailabilityDto> Availability { get; set; }
    public List<string> ImageUrls { get; set; }
}
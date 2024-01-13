namespace ECommerce.Models.DTOs.Product;
public class ProductDetailsDto : ProductDto
{
    public List<ReviewDto> Reviews { get; set; } 
    
    public AvailabilityDto Availability { get; set; }
    
}

public class AvailabilityDto
{
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
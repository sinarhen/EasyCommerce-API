namespace ECommerce.Models.DTOs;

public class ProductDetailsDto : ProductDto
{
    public List<ReviewDto> Reviews { get; set; }
    
}

public class ProductDetailsWithStockDto : ProductDetailsDto
{
    public List<ProductStockDto> Stocks { get; set; }
}

public class ProductDetailsWithSingleStockDto : ProductDetailsDto
{
    public ProductStockDto Stock { get; set; }
}



public class ReviewDto
{
    public string CustomerName { get; set; } 
    public string Title { get; set; }
    public string Content { get; set; }
    public int Rating { get; set; }
    public DateTime CreatedAt { get; set; }
}

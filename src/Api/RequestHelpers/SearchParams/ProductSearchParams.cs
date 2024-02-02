namespace ECommerce.RequestHelpers;

public class ProductSearchParams
{
    public string OrderBy { get; set; }
    
    public string FilterBy { get; set; }

    public int PageSize { get; set; } = 10;

    public int PageNumber { get; set; } = 1;
    
    public string SearchTerm { get; set; }
    
    public Guid CategoryId { get; set; }
    
    public string Color { get; set; }
    
    public string Size { get; set; }
    
    public Guid CollectionId { get; set; }
    
    public string Material { get; set; }
    
    public string Occasion { get; set; }

    public decimal MinPrice { get; set; } = 0;

    public decimal MaxPrice { get; set; } = decimal.MaxValue;

    
}
namespace ECommerce.RequestHelpers;

public class ProductSearchParams
{
    public Guid? ProductId { get; set; }
    public string OrderBy { get; set; }
    
    public string FilterBy { get; set; }

    public int PageSize { get; set; } = 10;

    public int PageNumber { get; set; } = 1;
    
    public string SearchTerm { get; set; }
    
    public Guid CategoryId { get; set; }
    
    public Guid ColorId { get; set; }
    
    public Guid SizeId { get; set; }
    
    public Guid CollectionId { get; set; }
    
    public Guid MaterialId { get; set; }
    
    public Guid OccasionId { get; set; }

    public decimal MinPrice { get; set; } = 0;

    public decimal MaxPrice { get; set; } = decimal.MaxValue;

    
}
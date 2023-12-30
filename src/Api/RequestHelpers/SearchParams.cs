namespace Ecommerce.RequestHelpers;

public class SearchParams
{
    public string OrderBy { get; set; }
    
    public string FilterBy { get; set; }
    
    public int? PageSize { get; set; }
    
    public int? PageNumber { get; set; }
    
    public string SearchTerm { get; set; }
    
    public string Category { get; set; }
    
    public string Color { get; set; }
    
    public string Size { get; set; }
    
    public string Material { get; set; }
    
    public string Occasion { get; set; }
    
}
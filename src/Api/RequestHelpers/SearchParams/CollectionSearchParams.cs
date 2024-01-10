namespace ECommerce.RequestHelpers.SearchParams;

public class CollectionSearchParams
{
    public string OrderBy { get; set; }
    public string FilterBy { get; set; }
    public string SearchTerm { get; set; }

    public decimal MinPrice { get; set; } = 0;

    public decimal MaxPrice { get; set; } = decimal.MaxValue;

}

namespace ECommerce.Models.DTOs.Billboard;

public class BillboardDto
{
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public string ImageUrl { get; set; }
    public string FilterTitle { get; set; }
    public string FilterSubtitle { get; set; }
    public BillboardFilterDto BillboardFilter { get; set; }
}

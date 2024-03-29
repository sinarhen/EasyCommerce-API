namespace ECommerce.Models.DTOs.Billboard;

public class BillboardDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public string ImageUrl { get; set; }
    public BillboardFilterDto BillboardFilter { get; set; }
}
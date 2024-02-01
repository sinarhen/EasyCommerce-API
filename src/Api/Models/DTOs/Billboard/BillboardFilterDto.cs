using ECommerce.Models.Entities;

namespace ECommerce.Models.DTOs.Billboard;
public class BillboardFilterDto
{
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public Gender? Gender { get; set; }
    public Season? Season { get; set; }
    public string OrderBy { get; set; }
    public decimal? FromPrice { get; set; } = 0;
    public decimal? ToPrice { get; set; } = decimal.MaxValue;
    public string Search { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? ColorId { get; set; }
    public Guid? SizeId { get; set; }
}


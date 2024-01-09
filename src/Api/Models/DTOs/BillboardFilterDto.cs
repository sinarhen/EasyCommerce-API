using ECommerce.Models.Entities;

namespace ECommerce.Models.DTOs;

public class BillboardFilterDto
{
    public Guid Id { get; set; }
    public Gender? Gender { get; set; }
    public Season? Season { get; set; }
    public string OrderBy { get; set; }
    public decimal? FromPrice { get; set; }
    public decimal? ToPrice { get; set; }
    public string Search { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? ColorId { get; set; }
    public Guid? SizeId { get; set; }
}
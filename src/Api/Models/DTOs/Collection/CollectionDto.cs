using ECommerce.Models.DTOs.Billboard;

namespace ECommerce.Models.DTOs.Collection;

public class CollectionDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<BillboardDto> Billboards { get; set; }
}
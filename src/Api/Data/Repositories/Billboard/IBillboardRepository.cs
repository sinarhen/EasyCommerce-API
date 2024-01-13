using ECommerce.Models.DTOs.Billboard;
using ECommerce.Models.Entities;

namespace ECommerce.Data.Repositories.Billboard;

public interface IBillboardRepository
{
    Task<ECommerce.Models.Entities.Billboard> GetBillboardAsync(Guid billboardId);
    Task<IEnumerable<ECommerce.Models.Entities.Billboard>> GetBillboardsForCollectionAsync(Guid collectionId);
    Task<ECommerce.Models.Entities.Billboard> CreateBillboardForCollectionAsync(CreateBillboardDto createBillboardDto);
    Task<ECommerce.Models.Entities.Billboard> UpdateBillboard(UpdateBillboardDto updateBillboardDto);
    Task DeleteBillboard(Guid billboardId);
}

public class UpdateBillboardDto : WriteBillboardDto
{
    
}

public class CreateBillboardDto : WriteBillboardDto
{
    
}


public class BillboardFilterDto
{
    
    public Gender? Gender { get; set; }

    public Guid? CategoryId { get; set; }

    public Season? Season { get; set; }

    public Guid? ColorId { get; set; }
    
    public string OrderBy { get; set; }
    
    public decimal? FromPrice { get; set; }
    
    public decimal? ToPrice { get; set; }
    
    public Guid? SizeId { get; set; }
    
    public string Search { get; set; }
    
}
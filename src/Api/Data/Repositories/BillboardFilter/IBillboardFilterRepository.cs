using ECommerce.Models.DTOs.Billboard;

namespace ECommerce.Data.Repositories.BillboardFilter;
public interface IBillboardFilterRepository
{
    Task<ECommerce.Models.Entities.BillboardFilter> GetBillboardFilterAsync(Guid id);
    Task<IEnumerable<ECommerce.Models.Entities.BillboardFilter>> GetBillboardFiltersAsync(BillboardFilterDto billboardFilterDto);
    Task<ECommerce.Models.Entities.BillboardFilter> CreateBillboardFilterAsync(BillboardFilterDto writeBillboardFilterDto);
    Task<ECommerce.Models.Entities.BillboardFilter> UpdateBillboardFilterAsync(Guid id, BillboardFilterDto writeBillboardFilterDto);
    Task DeleteBillboardFilterAsync(Guid id);
}
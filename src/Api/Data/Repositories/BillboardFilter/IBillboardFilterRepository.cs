using ECommerce.Models.DTOs.Billboard;

namespace ECommerce.Data.Repositories.BillboardFilter;
public interface IBillboardFilterRepository
{
    Task<ECommerce.Models.Entities.BillboardFilter> GetBillboardFilterAsync(string userId, Guid id, bool? isAdmin);
    Task<ECommerce.Models.Entities.BillboardFilter> CreateBillboardFilterAsync(Guid billboardId, string userId, BillboardFilterDto writeBillboardFilterDto, bool? isAdmin);
    Task<ECommerce.Models.Entities.BillboardFilter> UpdateBillboardFilterAsync(string userId, Guid id, BillboardFilterDto writeBillboardFilterDto, bool? isAdmin);
    Task DeleteBillboardFilterAsync(string userId, Guid id, bool? isAdmin);
}
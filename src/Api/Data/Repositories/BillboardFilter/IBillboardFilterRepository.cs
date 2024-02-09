using ECommerce.Models.DTOs.Billboard;

namespace ECommerce.Data.Repositories.BillboardFilter;

public interface IBillboardFilterRepository
{
    Task<Models.Entities.BillboardFilter> GetBillboardFilterAsync(string userId, Guid id, bool isAdmin = false);

    Task<Models.Entities.BillboardFilter> CreateBillboardFilterAsync(Guid billboardId, string userId,
        BillboardFilterDto writeBillboardFilterDto, bool isAdmin = false);

    Task<Models.Entities.BillboardFilter> UpdateBillboardFilterAsync(string userId, Guid id,
        BillboardFilterDto writeBillboardFilterDto, bool isAdmin = false);

    Task DeleteBillboardFilterAsync(string userId, Guid id, bool? isAdmin = false);
}
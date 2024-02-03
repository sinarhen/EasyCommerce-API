using ECommerce.Models.DTOs.Billboard;

namespace ECommerce.Data.Repositories.Billboard;

public interface IBillboardRepository
{
    Task<ECommerce.Models.Entities.Billboard> GetBillboardAsync(Guid billboardId);
    Task<IEnumerable<ECommerce.Models.Entities.Billboard>> GetBillboardsForCollectionAsync(Guid collectionId);
    Task<ECommerce.Models.Entities.Billboard> CreateBillboardForCollectionAsync(Guid collectionId, string userId, CreateBillboardDto createBillboardDto, bool isAdmin);
    Task<ECommerce.Models.Entities.Billboard> UpdateBillboardAsync(Guid billboardId, UpdateBillboardDto updateBillboardDto, string userId, bool isAdmin);
    Task DeleteBillboardAsync(Guid collectionId, Guid billboardId, string userId, bool isAdmin);
}

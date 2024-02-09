using ECommerce.Models.DTOs.Billboard;

namespace ECommerce.Data.Repositories.Billboard;

public interface IBillboardRepository
{
    Task<Models.Entities.Billboard> GetBillboardAsync(Guid billboardId);
    Task<IEnumerable<Models.Entities.Billboard>> GetBillboardsForCollectionAsync(Guid collectionId);

    Task<Models.Entities.Billboard> CreateBillboardForCollectionAsync(Guid collectionId, string userId,
        CreateBillboardDto createBillboardDto, bool isAdmin);

    Task<Models.Entities.Billboard> UpdateBillboardAsync(Guid billboardId, UpdateBillboardDto updateBillboardDto,
        string userId, bool isAdmin);

    Task DeleteBillboardAsync(Guid collectionId, Guid billboardId, string userId, bool isAdmin);
}
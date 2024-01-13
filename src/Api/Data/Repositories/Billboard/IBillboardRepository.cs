using ECommerce.Models.DTOs.Billboard;
using ECommerce.Models.Entities;

namespace ECommerce.Data.Repositories.Billboard;

public interface IBillboardRepository
{
    Task<ECommerce.Models.Entities.Billboard> GetBillboardAsync(Guid billboardId);
    Task<IEnumerable<ECommerce.Models.Entities.Billboard>> GetBillboardsForCollectionAsync(Guid collectionId);
    Task<ECommerce.Models.Entities.Billboard> CreateBillboardForCollectionAsync(CreateBillboardDto createBillboardDto);
    Task<ECommerce.Models.Entities.Billboard> UpdateBillboardAsync(UpdateBillboardDto updateBillboardDto, string userId, Guid billboardId);
    Task DeleteBillboard(Guid billboardId);
}

namespace ECommerce.Data.Repositories.Billboard;

public interface IBillboardRepository
{
    Task<ECommerce.Models.Entities.Billboard> GetBillboardAsync(Guid billboardId);
    Task<IEnumerable<ECommerce.Models.Entities.Billboard>> GetBillboardsForCollectionAsync(Guid collectionId);
    Task<ECommerce.Models.Entities.Billboard> CreateBillboardForCollectionAsync(/* CreateBillboardDto */);
    Task<ECommerce.Models.Entities.Billboard> UpdateBillboard(/* TODO: UpdateBillboardDto */);
    Task DeleteBillboard(Guid billboardId);
}
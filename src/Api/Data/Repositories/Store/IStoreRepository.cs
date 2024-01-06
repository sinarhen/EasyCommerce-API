using ECommerce.Models.DTOs;

namespace ECommerce.Data.Repositories.Store;

public interface IStoreRepository
{
    Task<Models.Entities.Store> GetStoreAsync(Guid id);
    Task<List<Models.Entities.Store>> GetStoresAsync();
    Task<Models.Entities.Store> CreateStoreAsync(StoreDto storeDto, string ownerId);
    Task<Models.Entities.Store> UpdateStoreAsync(Models.Entities.Store store);
    Task<Models.Entities.Store> DeleteStoreAsync(Guid id);
}

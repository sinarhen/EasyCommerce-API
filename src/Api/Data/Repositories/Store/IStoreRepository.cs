using ECommerce.Models.DTOs.Store;

namespace ECommerce.Data.Repositories.Store;

public interface IStoreRepository
{
    Task<Models.Entities.Store> GetStoreAsync(Guid id);
    Task<List<Models.Entities.Store>> GetStoresForUserAsync(string userId);
    Task<List<Models.Entities.Store>> GetStoresAsync();
    Task<Models.Entities.Store> CreateStoreAsync(StoreDto storeDto, string ownerId);
    Task UpdateStoreAsync(Guid storeId, StoreDto storeDto, string userId, bool isAdmin);
    Task DeleteStoreAsync(Guid id, string userId, bool isAdmin);
}
using ECommerce.Models.DTOs;
using ECommerce.Models.DTOs.Store;

namespace ECommerce.Data.Repositories.Store;

public interface IStoreRepository
{
    Task<Models.Entities.Store> GetStoreAsync(Guid id);
    Task<List<Models.Entities.Store>> GetStoresAsync();
    Task<Models.Entities.Store> CreateStoreAsync(StoreDto storeDto, string ownerId);
    Task UpdateStoreAsync(Guid id, StoreDto storeDto);
    Task DeleteStoreAsync(Guid id);
}

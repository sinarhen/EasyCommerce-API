using ECommerce.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Data.Repositories.Store;

public class StoreRepository : BaseRepository, IStoreRepository
{
    public StoreRepository(ProductDbContext db) : base(db)
    {
        
    }
    public async Task<Models.Entities.Store> GetStoreAsync(Guid id)
    {
        return await _db.Stores.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<List<Models.Entities.Store>> GetStoresAsync()
    {
        return await _db.Stores.ToListAsync();
    }

    public async Task<Models.Entities.Store> CreateStoreAsync(StoreDto storeDto, string ownerId)
    {
        if (storeDto == null)
        {
            throw new ArgumentNullException(nameof(storeDto));
        }
        if (string.IsNullOrEmpty(storeDto.Name))
        {
            throw new ArgumentException("Store name cannot be empty");
        }
        if (string.IsNullOrEmpty(storeDto.Email))
        {
            throw new ArgumentException("Email cannot be empty");
        }

        if (string.IsNullOrEmpty(ownerId))
        {
            throw new Exception("User id not provided. Internal Error");
        }
        var store = new Models.Entities.Store
        {
            Name = storeDto.Name,
            Description = storeDto.Description,
            BannerUrl = storeDto.BannerUrl,
            LogoUrl = storeDto.LogoUrl,
            Address = storeDto.Address,
            Contacts = storeDto.Contacts,
            Email = storeDto.Email,
            OwnerId = ownerId
        };
        
        await _db.Stores.AddAsync(store);
        await SaveChangesAsyncWithTransaction();
        
        return store;
    }

    public async Task<Models.Entities.Store> UpdateStoreAsync(Models.Entities.Store store)
    {
        throw new NotImplementedException();
    }

    public async Task<Models.Entities.Store> DeleteStoreAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}
using ECommerce.Models.DTOs.Store;
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
        return await _db.Stores
            .AsNoTracking()
            .Include(s => s.Collections)
            .ThenInclude(c => c.Billboards)
            .ThenInclude(b => b.BillboardFilter)
            .Include(s => s.Owner)
            .ToListAsync();
    }

    public async Task<Models.Entities.Store> CreateStoreAsync(StoreDto storeDto, string ownerId)
    {
        if (string.IsNullOrEmpty(ownerId)) throw new Exception("User id not provided. Internal Error");

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

    public async Task UpdateStoreAsync(Guid storeId, StoreDto storeDto, string userId, bool isAdmin)
    {
        var store = await _db.Stores.FirstOrDefaultAsync(s => s.Id == storeId);
        if (store == null) throw new ArgumentException($"Store not found: {storeId}");
        var isOwner = ValidateOwner(store.OwnerId, userId, isAdmin);
        if (!isOwner) throw new UnauthorizedAccessException("You are not authorized to update this store");
        store.Name = storeDto.Name;
        store.Description = storeDto.Description;
        store.BannerUrl = storeDto.BannerUrl;
        store.LogoUrl = storeDto.LogoUrl;
        store.Address = storeDto.Address;
        store.Contacts = storeDto.Contacts;
        store.Email = storeDto.Email;

        await SaveChangesAsyncWithTransaction();
    }

    public async Task DeleteStoreAsync(Guid id, string userId, bool isAdmin)
    {
        var store = await _db.Stores
            .Include(store => store.Collections)
            .ThenInclude(collection => collection.Products)
            .ThenInclude(products => products.Categories)
            .Include(store => store.Collections)
            .ThenInclude(collection => collection.Products)
            .ThenInclude(products => products.Materials)
            .Include(store => store.Collections)
            .ThenInclude(collection => collection.Products)
            .ThenInclude(products => products.Stocks)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (store == null) throw new ArgumentException($"Store not found: {id}");
        var isOwner = ValidateOwner(store.OwnerId, userId, isAdmin);
        if (!isOwner) throw new UnauthorizedAccessException("You are not authorized to delete this store");

        _db.Stores.Remove(store);
        await SaveChangesAsyncWithTransaction();
    }

    public async Task<List<Models.Entities.Store>> GetStoresForUserAsync(string userId)
    {
        return await _db.Stores
            .AsNoTracking()
            .Where(s => s.OwnerId == userId)
            .Include(s => s.Collections)
            .ThenInclude(c => c.Billboards)
            .ThenInclude(b => b.BillboardFilter)
            .Include(s => s.Owner)
            .ToListAsync();
    }
}
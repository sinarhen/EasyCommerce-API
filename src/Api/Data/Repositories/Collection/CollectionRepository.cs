using ECommerce.Config;
using ECommerce.Models.DTOs.Collection;
using ECommerce.RequestHelpers.SearchParams;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Data.Repositories.Collection;

public class CollectionRepository : BaseRepository, ICollectionRepository
{
    public CollectionRepository(ProductDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Models.Entities.Collection> CreateCollectionAsync(CreateCollectionDto collection, string ownerId,
        List<string> ownerRoles)
    {
        if (collection == null) throw new ArgumentException("Request body is empty");
        if (Guid.Empty.Equals(ownerId)) throw new UnauthorizedAccessException("OwnerId is empty. Internal error");
        if (collection.StoreId == Guid.Empty) throw new ArgumentException("StoreId is empty");

        var store = await _db.Stores.FindAsync(collection.StoreId) ?? throw new ArgumentException("Store not found");

        // If the user is a Seller, check if they are the owner of the store
        if (ownerRoles.Contains(UserRoles.Seller) && !ownerRoles.Contains(UserRoles.Admin) &&
            !ownerRoles.Contains(UserRoles.SuperAdmin))
        {
            var isOwner = store.OwnerId == ownerId;
            if (!isOwner) throw new UnauthorizedAccessException("You are not the owner of any store");
        }


        var coll = new Models.Entities.Collection
        {
            Name = collection.Name,
            Description = collection.Description
        };
        await _db.Collections.AddAsync(coll);

        await SaveChangesAsyncWithTransaction();

        return coll;
    }


    public async Task DeleteCollectionAsync(Guid collectionId, string ownerId, List<string> ownerRoles)
    {
        if (Guid.Empty.Equals(collectionId)) throw new ArgumentException("Id is empty. Internal error");
        if (Guid.Empty.Equals(ownerId)) throw new UnauthorizedAccessException("OwnerId is empty. Internal error");

        var existingCollection = await _db.Collections
                                     .Include(c => c.Store)
                                     .FirstOrDefaultAsync(c => collectionId == c.Id)
                                 ?? throw new ArgumentException("Collection not found");

        // If the user is a Seller, check if they are the owner of the collection
        if (ownerRoles.Contains(UserRoles.Seller) && !ownerRoles.Contains(UserRoles.Admin) &&
            !ownerRoles.Contains(UserRoles.SuperAdmin))
            if (existingCollection.Store.OwnerId != ownerId)
                throw new UnauthorizedAccessException("You are not the owner of this collection");

        _db.Collections.Remove(existingCollection);
        await _db.SaveChangesAsync();
    }

    public async Task<Models.Entities.Collection> GetCollectionByIdAsync(Guid id)
    {
        var collection = await _db.Collections
                             .Include(c => c.Billboards)
                             .ThenInclude(b => b.BillboardFilter)
                             .AsNoTracking().AsSplitQuery().FirstOrDefaultAsync(c => c.Id == id) ??
                         throw new ArgumentException("Collection not found");
        return collection;
    }

    public async Task<IEnumerable<Models.Entities.Collection>> GetRandomCollectionsAsync()
    {
        return await _db.Collections
            .AsSplitQuery()
            .AsNoTracking()
            .AsQueryable()
            .OrderBy(c => Guid.NewGuid())
            .Take(10)
            .ToListAsync();
    }

    public async Task UpdateCollectionAsync(Guid collectionId, CreateCollectionDto collection, string ownerId,
        List<string> ownerRoles)
    {
        if (collection == null) throw new ArgumentException("Request body is empty");

        if (Guid.Empty.Equals(collectionId)) throw new ArgumentException("Id is empty. Internal error");
        if (Guid.Empty.Equals(ownerId)) throw new UnauthorizedAccessException("OwnerId is empty. Internal error");

        var existingCollection = await _db.Collections
                                     .Include(c => c.Store)
                                     .FirstOrDefaultAsync(c => collectionId == c.Id)
                                 ?? throw new ArgumentException("Collection not found");

        // If the user is a Seller, check if they are the owner of the collection
        if (ownerRoles.Contains(UserRoles.Seller) && !ownerRoles.Contains(UserRoles.Admin) &&
            !ownerRoles.Contains(UserRoles.SuperAdmin))
            if (existingCollection.Store.OwnerId != ownerId)
                throw new UnauthorizedAccessException("You are not the owner of this collection");

        if (!string.IsNullOrEmpty(collection.Name)) existingCollection.Name = collection.Name;

        _db.Collections.Update(existingCollection);
        await _db.SaveChangesAsync();
    }

    public async Task<IEnumerable<Models.Entities.Collection>> GetCollectionsAsync(CollectionSearchParams searchParams)
    {
        var query = _db.Collections
                .AsSplitQuery()
                .AsNoTracking()
                .AsQueryable()
            ;

        if (!string.IsNullOrEmpty(searchParams.SearchTerm))
            query = query.Where(c => c.Name.Contains(searchParams.SearchTerm));


        var includedStocks = false;
        if (searchParams.MinPrice > 0)
        {
            query = query.Include(c => c.Products).ThenInclude(p => p.Stocks);

            query = query.Where(
                c => c.Products.Min(p => p.Stocks.Min(s => s.Price)) >= searchParams.MinPrice);
        }

        if (searchParams.MaxPrice < decimal.MaxValue)
        {
            if (!includedStocks) query = query.Include(c => c.Products).ThenInclude(p => p.Stocks);
            query = query.Where(c => c.Products.Max(p => p.Stocks.Max(s => s.Price)) <= searchParams.MaxPrice);
        }

        query = searchParams.OrderBy switch
        {
            "name" => query.OrderBy(c => c.Name),
            "name_desc" => query.OrderByDescending(c => c.Name),
            _ => query.OrderBy(c => c.Name)
        };

        query = searchParams.FilterBy switch
        {
            "new" => query.Where(c => c.CreatedAt >= DateTime.Now.AddDays(-7)),
            "verified" => query.Where(c => c.Store.IsVerified),
            "has_sale" => query.Where(c => c.HasSale),
            _ => query
        };

        return await query.ToListAsync();
    }
}
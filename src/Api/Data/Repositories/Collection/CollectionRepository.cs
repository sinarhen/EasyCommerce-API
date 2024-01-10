using ECommerce.Models.DTOs;
using ECommerce.RequestHelpers;
using ECommerce.RequestHelpers.SearchParams;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Data.Repositories.Collection;

public class CollectionRepository : BaseRepository, ICollectionRepository
{
    public CollectionRepository(ProductDbContext dbContext) : base(dbContext)
    {

    }

    public async Task<ECommerce.Models.Entities.Collection> CreateCollectionAsync(CreateCollectionDto collection, Guid storeId)
    {
        var coll = new ECommerce.Models.Entities.Collection
        {
            Name = collection.Name,
            Description = collection.Description,
            StoreId = storeId
        };
        await _db.Collections.AddAsync(coll);

        await SaveChangesAsyncWithTransaction();

        return coll;
    }

    public async Task DeleteCollectionAsync(Guid id)
    {
        var collection = await _db.Collections.FindAsync(id);
        if (collection == null)
        {
            throw new ArgumentException("Collection not found");
        }

        _db.Collections.Remove(collection);
        await SaveChangesAsyncWithTransaction();
    }

    public async Task<ECommerce.Models.Entities.Collection> GetCollectionByIdAsync(Guid id)
    {
        var collection = await _db.Collections.FindAsync(id);
        if (collection == null)
        {
            throw new ArgumentException("Collection not found");
        }
        return collection;
    }

    public async Task<IEnumerable<ECommerce.Models.Entities.Collection>> GetCollectionsAsync(CollectionSearchParams searchParams)
    {
        var query = _db.Collections
            .AsSplitQuery()
            .AsNoTracking()
            .AsQueryable()
            ;

        if (!string.IsNullOrEmpty(searchParams.SearchTerm))
        {
            query = query.Where(c => c.Name.Contains(searchParams.SearchTerm));
        }


        bool includedStocks = false;
        if (searchParams.MinPrice > 0)
        {
            query = query.Include(c => c.Products).ThenInclude(p => p.Stocks);

            query = query.Where(
                c => c.Products.Min(p => p.Stocks.Min(s => s.Price)) >= searchParams.MinPrice);
        }

        if (searchParams.MaxPrice < decimal.MaxValue)
        {
            if (!includedStocks)
            {
                query = query.Include(c => c.Products).ThenInclude(p => p.Stocks);
            }
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

    public async Task UpdateCollectionAsync(Guid collectionId, CreateCollectionDto collection, string ownerId)
    {
        if (collection == null)
        {
            throw new ArgumentException("Request body is empty");
        }
        
        if (Guid.Empty.Equals(collectionId))
        {
            throw new ArgumentException("Id is empty. Internal error");
        }
        if (Guid.Empty.Equals(ownerId))
        {
            throw new UnauthorizedAccessException("OwnerId is empty. Internal error");
        }

        var existingCollection = await _db.Collections
            .Include(c => c.Store)
            .FirstOrDefaultAsync(c => collectionId == c.Id) 
        ?? throw new ArgumentException("Collection not found");
        

        if (existingCollection.Store.OwnerId != ownerId)
        {
            throw new UnauthorizedAccessException("You are not the owner of this collection");
        }

        if (!string.IsNullOrEmpty(collection.Name))
        {
            existingCollection.Name = collection.Name;
        }

        if (!string.IsNullOrEmpty(collection.Description))
        {
            existingCollection.Description = collection.Description;
        }

        _db.Collections.Update(existingCollection);

        await SaveChangesAsyncWithTransaction();
    }
}
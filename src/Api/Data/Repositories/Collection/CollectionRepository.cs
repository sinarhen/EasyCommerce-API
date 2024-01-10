
using System.Collections;
using ECommerce.Config;
using ECommerce.Models.DTOs;
using Microsoft.AspNetCore.Authorization;

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

    public Task DeleteCollectionAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection> GetCollectionByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ICollection>> GetCollectionsAsync()
    {
        throw new NotImplementedException();
    }

    public Task UpdateCollectionAsync(ECommerce.Models.Entities.Collection collection)
    {
        throw new NotImplementedException();
    }
}
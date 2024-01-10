﻿using ECommerce.Models.DTOs;
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

    public async Task<IEnumerable<ECommerce.Models.Entities.Collection>> GetCollectionsAsync()
    {
        return await _db.Collections.AsNoTracking()
            .Include(c => c.Billboards).ThenInclude(b => b.BillboardFilter)
            .ToListAsync();
    }

    public async Task UpdateCollectionAsync(Guid id, CreateCollectionDto collection)
    {
        if (collection == null)
        {
            throw new ArgumentException("Request body is empty");
        }

        if (Guid.Empty.Equals(id))
        {
            throw new ArgumentException("Id is empty. Internal error");
        }

        var existingCollection = await _db.Collections.FindAsync(id) ?? throw new ArgumentException("Collection not found");
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
using System.Collections;
using System.Collections.Generic;
using ECommerce.Models.DTOs;


namespace ECommerce.Data.Repositories.Collection;

public interface ICollectionRepository
{
    Task<ECommerce.Models.Entities.Collection> GetCollectionByIdAsync(Guid id);
    Task<IEnumerable<ECommerce.Models.Entities.Collection>> GetCollectionsAsync();
    Task<ECommerce.Models.Entities.Collection> CreateCollectionAsync(CreateCollectionDto collection, Guid storeId);
    Task UpdateCollectionAsync(Guid collectionId, CreateCollectionDto collection, string ownerId); // The same dto is used for update and create
    Task DeleteCollectionAsync(Guid id);


}


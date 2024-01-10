using System.Collections;
using System.Collections.Generic;
using ECommerce.Models.DTOs;
using ECommerce.Models.Entities;
using ECommerce.RequestHelpers;
using ECommerce.RequestHelpers.SearchParams;


namespace ECommerce.Data.Repositories.Collection;

public interface ICollectionRepository
{
    Task<ECommerce.Models.Entities.Collection> GetCollectionByIdAsync(Guid id);
    Task<IEnumerable<ECommerce.Models.Entities.Collection>> GetRandomCollectionsAsync();
    Task<IEnumerable<ECommerce.Models.Entities.Collection>> GetCollectionsForStoreAsync(Guid storeId);
    Task<IEnumerable<ECommerce.Models.Entities.Collection>> GetCollectionsAsync(CollectionSearchParams searchParams);
    Task<ECommerce.Models.Entities.Collection> CreateCollectionAsync(CreateCollectionDto collection, Guid storeId);
    Task UpdateCollectionAsync(Guid storeId, Guid collectionId, CreateCollectionDto collection, string ownerId); // The same dto is used for update and create
    Task DeleteCollectionAsync(Guid storeId, Guid id, string ownerId);
    Task<IEnumerable<ECommerce.Models.Entities.Product>> GetProductsInCollectionAsync(Guid storeId, Guid collectionId, ProductSearchParams searchParams);

    // ownerId used for authorization of the request. Only the owner of the store can update or delete the collection

}


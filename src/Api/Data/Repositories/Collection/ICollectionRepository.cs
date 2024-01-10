using System.Collections;
using System.Collections.Generic;
using ECommerce.Models.DTOs;


namespace ECommerce.Data.Repositories.Collection;

public interface ICollectionRepository
{
    Task<ICollection> GetCollectionByIdAsync(Guid id);
    Task<IEnumerable<ICollection>> GetCollectionsAsync();
    Task<ECommerce.Models.Entities.Collection> CreateCollectionAsync(CreateCollectionDto collection, Guid storeId);
    Task UpdateCollectionAsync(ECommerce.Models.Entities.Collection collection); // The same dto is used for update and create
    Task DeleteCollectionAsync(Guid id);


}


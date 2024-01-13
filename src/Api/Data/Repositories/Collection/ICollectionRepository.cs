using System.Collections;
using System.Collections.Generic;
using ECommerce.Models.DTOs;
using ECommerce.Models.DTOs.Collection;
using ECommerce.Models.Entities;
using ECommerce.RequestHelpers;
using ECommerce.RequestHelpers.SearchParams;


namespace ECommerce.Data.Repositories.Collection;

public interface ICollectionRepository
{
    Task<ECommerce.Models.Entities.Collection> GetCollectionByIdAsync(Guid id);
    Task<IEnumerable<ECommerce.Models.Entities.Collection>> GetRandomCollectionsAsync();
    Task<ECommerce.Models.Entities.Collection> CreateCollectionAsync(CreateCollectionDto collectionDto, string ownerId, List<string> ownerRoles);
    Task UpdateCollectionAsync(Guid id, CreateCollectionDto collectionDto, string ownerId, List<string> ownerRoles);
    Task DeleteCollectionAsync(Guid id, string ownerId, List<string> ownerRoles);
}
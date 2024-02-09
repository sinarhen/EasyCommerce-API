using ECommerce.Models.DTOs.Collection;

namespace ECommerce.Data.Repositories.Collection;

public interface ICollectionRepository
{
    Task<Models.Entities.Collection> GetCollectionByIdAsync(Guid id);
    Task<IEnumerable<Models.Entities.Collection>> GetRandomCollectionsAsync();

    Task<Models.Entities.Collection> CreateCollectionAsync(CreateCollectionDto collectionDto, string ownerId,
        List<string> ownerRoles);

    Task UpdateCollectionAsync(Guid id, CreateCollectionDto collectionDto, string ownerId, List<string> ownerRoles);
    Task DeleteCollectionAsync(Guid id, string ownerId, List<string> ownerRoles);
}
using AutoMapper;
using ECommerce.Data.Repositories.Collection;
using ECommerce.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers.Collection;


[ApiController]
[Route("api/collections")]
public class CollectionController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ICollectionRepository _repository;

    // Other dependencies here...

    public CollectionController(IMapper mapper, ICollectionRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }



    // This endpoint handles "api/collections" and returns random collections from different sellers
    [HttpGet("api/collections")]
    public async Task<ActionResult> GetRandomCollections()
    {

        return Ok();
    }

    // These endpoints handle "api/stores/{storeId}/collections" for CRUD operations on a specific store's collections

    [HttpGet("api/stores/{storeId}/collections")]
    public async Task<ActionResult> GetCollectionsForStore(Guid storeId)
    {
        // Implementation here...
        return Ok();

    }

    [HttpPost("api/stores/{storeId}/collections")]
    public async Task<ActionResult> CreateCollectionForStore(Guid storeId, CollectionDto collectionDto)
    {
        // Implementation here...
        return Ok();

    }

    [HttpPut("api/stores/{storeId}/collections/{id}")]
    public async Task<ActionResult> UpdateCollectionForStore(Guid storeId, Guid id, CollectionDto collectionDto)
    {
        // Implementation here...
        return Ok();

    }

    [HttpDelete("api/stores/{storeId}/collections/{id}")]
    public async Task<ActionResult> DeleteCollectionForStore(Guid storeId, Guid id)
    {
        // Implementation here...
        return Ok();
    }
}

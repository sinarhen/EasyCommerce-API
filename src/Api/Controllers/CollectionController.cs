using AutoMapper;
using ECommerce.Config;
using ECommerce.Data.Repositories.Collection;
using ECommerce.Models.DTOs.Collection;
using ECommerce.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers;

[ApiController]
[Route("api/collections")]
public class CollectionController : GenericController
{
    private readonly ICollectionRepository _repository;

    public CollectionController(IMapper mapper, ICollectionRepository repository) : base(mapper)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<List<CollectionDto>>> GetCollections()
    {
        var collections = await _repository.GetRandomCollectionsAsync();
        return Ok(_mapper.Map<List<CollectionDto>>(collections));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetCollection(Guid id)
    {
        var collection = await _repository.GetCollectionByIdAsync(id);
        return Ok(_mapper.Map<CollectionDto>(collection));
    }


    [HttpPatch("{id:guid}")]
    [Authorize(Policy = Policies.SellerPolicy)]
    [ServiceFilter(typeof(ValidationService))]
    public async Task<ActionResult> UpdateCollection(Guid id, CreateCollectionDto collectionDto)
    {
        var ownerId = GetUserId();
        var ownerRoles = GetUserRoles();
        await _repository.UpdateCollectionAsync(id, collectionDto, ownerId, ownerRoles);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteCollection(Guid id)
    {
        var ownerId = GetUserId();
        var ownerRoles = GetUserRoles();
        await _repository.DeleteCollectionAsync(id, ownerId, ownerRoles);
        return NoContent();
    }

    [HttpPost]
    [Authorize(Policy = Policies.SellerPolicy)]
    [ServiceFilter(typeof(ValidationService))]
    public async Task<ActionResult> CreateCollection(CreateCollectionDto collectionDto)
    {
        var ownerId = GetUserId();
        var ownerRoles = GetUserRoles();
        var collection = await _repository.CreateCollectionAsync(collectionDto, ownerId, ownerRoles);
        return CreatedAtAction(nameof(CreateCollection), new { id = collection.Id }, collection);
    }
}
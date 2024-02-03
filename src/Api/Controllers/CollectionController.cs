using System.Security.Claims;
using AutoMapper;
using ECommerce.Config;
using ECommerce.Data.Repositories.Collection;
using ECommerce.Models.DTOs.Collection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers.Collection;


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
        try
        {
            var collections = await _repository.GetRandomCollectionsAsync();
            return Ok(_mapper.Map<List<CollectionDto>>(collections));
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetCollection(Guid id)
    {
        var collection = await _repository.GetCollectionByIdAsync(id);
        return Ok(_mapper.Map<CollectionDto>(collection));
    }


    [HttpPut("{id}")]
    [Authorize(Policy = Policies.SellerPolicy)]
    public async Task<ActionResult> UpdateCollection(Guid id, CreateCollectionDto collectionDto)
    {
        try
        {
            var ownerId = GetUserId();
            var ownerRoles = GetUserRoles();            await _repository.UpdateCollectionAsync(id, collectionDto, ownerId, ownerRoles);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCollection(Guid id)
    {
        try
        {
            var ownerId = GetUserId();
            var ownerRoles = GetUserRoles();            
            await _repository.DeleteCollectionAsync(id, ownerId, ownerRoles);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Authorize(Policy = Policies.SellerPolicy)]
    public async Task<ActionResult> CreateCollection(CreateCollectionDto collectionDto)
    {
        try
        {
            var ownerId = GetUserId();
            var ownerRoles = GetUserRoles();            var collection = await _repository.CreateCollectionAsync(collectionDto, ownerId, ownerRoles);
            return CreatedAtAction(nameof(CreateCollection), new { id = collection.Id }, collection);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}
using System.Security.Claims;
using AutoMapper;
using ECommerce.Config;
using ECommerce.Data.Repositories.Collection;
using ECommerce.Models.DTOs;
using ECommerce.RequestHelpers;
using ECommerce.RequestHelpers.SearchParams;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers.Collection;


[ApiController]
[Route("api/collections")]
public class CollectionController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ICollectionRepository _repository;

    public CollectionController(IMapper mapper, ICollectionRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }
    [HttpGet("api/collections/{id}")]
    public async Task<ActionResult> GetCollection(Guid id)
    {
        var collection = await _repository.GetCollectionByIdAsync(id);
        return Ok(_mapper.Map<CollectionDto>(collection));
    }
    [HttpPut("api/collections/{id}")]
    [Authorize(Roles = UserRoles.Admin + "," + UserRoles.SuperAdmin + "," + UserRoles.Seller)]
    public async Task<ActionResult> UpdateCollection(Guid id, CreateCollectionDto collectionDto)
    {
        try
        {
            var ownerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var ownerRoles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
            await _repository.UpdateCollectionAsync(id, collectionDto, ownerId, ownerRoles);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("api/collections/{id}")]
    public async Task<ActionResult> DeleteCollection(Guid id)
    {
        try
        {
            var ownerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var ownerRoles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
            await _repository.DeleteCollectionAsync(id, ownerId, ownerRoles);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}
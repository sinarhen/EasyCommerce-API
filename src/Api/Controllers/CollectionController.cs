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

    [HttpGet]
    public async Task<ActionResult> GetRandomCollections()
    {
        try 
        {
            var collections = await _repository.GetRandomCollectionsAsync();
            return Ok(_mapper.Map<IEnumerable<CollectionDto>>(collections));
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("api/stores/{storeId}/collections")]
    public async Task<ActionResult> GetCollectionsForStore(Guid storeId)
    {
        try 
        {
            var collections = await _repository.GetCollectionsForStoreAsync(storeId);
            return Ok(_mapper.Map<IEnumerable<CollectionDto>>(collections));
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost("api/stores/{storeId}/collections")]
    public async Task<ActionResult> CreateCollectionForStore(Guid storeId, CreateCollectionDto collectionDto)
    {
        try 
        {
            var collection = await _repository.CreateCollectionAsync(collectionDto, storeId);
            return CreatedAtAction(nameof(GetCollectionsForStore), new { storeId = storeId }, _mapper.Map<CollectionDto>(collection));
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPut("api/stores/{storeId}/collections/{id}")]
    public async Task<ActionResult> UpdateCollectionForStore(Guid storeId, Guid id, CreateCollectionDto collectionDto)
    {
        try 
        {
            var tokenId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (tokenId == null)
            {
                Console.WriteLine("Invalid token. No id claim found");
                return Unauthorized();
            }

            await _repository.UpdateCollectionAsync(storeId, id, collectionDto, tokenId);
            return NoContent();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Seller + "," + UserRoles.SuperAdmin)]
    [HttpDelete("api/stores/{storeId}/collections/{id}")]
    public async Task<ActionResult> DeleteCollectionForStore(Guid storeId, Guid id)
    {
        try 
        {
            var tokenId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (tokenId == null)
            {
                Console.WriteLine("Invalid token. No id claim found");
                return Unauthorized();
            }
            await _repository.DeleteCollectionAsync(storeId, id, tokenId);
            return NoContent();
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (UnauthorizedAccessException  e)
        {
            return Unauthorized(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("api/stores/{storeId}/collections/{id}/products")]
    public async Task<ActionResult> GetProductsForCollection(Guid storeId, Guid id, [FromQuery] ProductSearchParams searchParams)
    {
        try 
        {
            var products = await _repository.GetProductsInCollectionAsync(storeId, id, searchParams);
            return Ok(_mapper.Map<IEnumerable<ProductDto>>(products));
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (UnauthorizedAccessException  e)
        {
            return Unauthorized(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}
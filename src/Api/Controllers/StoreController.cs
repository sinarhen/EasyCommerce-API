using AutoMapper;
using ECommerce.Config;
using ECommerce.Data.Repositories.Store;
using ECommerce.Models.DTOs.Store;
using ECommerce.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers;

[ApiController]
[Route("api/stores")]
public class StoreController : GenericController
{
    private readonly IStoreRepository _repository;

    public StoreController(IMapper mapper, IStoreRepository repository) : base(mapper)
    {
        _repository = repository;
    }

    [Authorize(Policy = Policies.SellerPolicy)]
    [HttpGet("my")]
    public async Task<ActionResult> GetMyStores()
    {
        var userId = GetUserId();
        var stores = await _repository.GetStoresForUserAsync(userId);
        if (stores == null) return NotFound();
        var storesDto = _mapper.Map<IEnumerable<StoreDto>>(stores);

        return Ok(new
            {
                Stores = storesDto
            }
        );
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StoreDto>> GetStore(Guid id)
    {
        try
        {
            var store = await _repository.GetStoreAsync(id);
            if (store == null) return NotFound();
            var storeDto = _mapper.Map<StoreDto>(store);

            return Ok(storeDto);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    // TODO: [Authorize(Policy = Policies.)] ??? 
    [HttpGet]
    public async Task<ActionResult> GetStores()
    {
        try
        {
            var stores = await _repository.GetStoresAsync();
            if (stores == null) return NotFound();
            var storesDto = _mapper.Map<IEnumerable<StoreDto>>(stores);

            return Ok(new
                {
                    Stores = storesDto
                }
            );
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [Authorize(Policy = Policies.SellerPolicy)]
    [HttpPost]
    [ServiceFilter(typeof(ValidationService))]
    public async Task<ActionResult> CreateStore(StoreDto storeDto)
    {
        try
        {
            await _repository.CreateStoreAsync(storeDto, GetUserId());
            return Ok();
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


    [Authorize(Policy = Policies.SellerPolicy)]
    [HttpPut("{id}")]
    [ServiceFilter(typeof(ValidationService))]
    public async Task<ActionResult> UpdateStore(Guid id, StoreDto storeDto)
    {
        try
        {
            await _repository.UpdateStoreAsync(id, storeDto, GetUserId(), IsAdmin());
            return Ok();
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


    [Authorize(Policy = Policies.SellerPolicy)]
    [HttpDelete("{id}")]
    [ServiceFilter(typeof(ValidationService))]
    public async Task<ActionResult> DeleteStore(Guid id)
    {
        try
        {
            var store = await _repository.GetStoreAsync(id);
            

            await _repository.DeleteStoreAsync(id, GetUserId(), IsAdmin());
            return Ok();
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
}
using AutoMapper;
using ECommerce.Config;
using ECommerce.Data.Repositories.Store;
using ECommerce.Models.DTOs;
using ECommerce.Models.DTOs.Store;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace ECommerce.Controllers;

[ApiController]
[Route("api/stores")]
public class StoreController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IStoreRepository _repository;

public StoreController(IMapper mapper, IStoreRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<StoreDto>> GetStore(Guid id)
    {
        var store = await _repository.GetStoreAsync(id);
        if (store == null)
        {
            return NotFound();
        }
        var storeDto = _mapper.Map<StoreDto>(store);
        
        return Ok(storeDto);
    }

    // [Authorize(Policy = Policies.)] ???
    [HttpGet]
    public async Task<ActionResult> GetStores()
    {
        var stores = await _repository.GetStoresAsync();
        if (stores == null)
        {
            return NotFound();
        }
        var storesDto = _mapper.Map<IEnumerable<StoreDto>>(stores);

        return Ok(new 
            { 
                Stores = storesDto
            }
        );
    }
    
    [Authorize(Policy = Policies.SellerPolicy)]
    [HttpPost]
    public async Task<ActionResult> CreateStore(StoreDto storeDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await _repository.CreateStoreAsync(storeDto, userId);
        return Ok();
    }
    
    
    [Authorize(Policy = Policies.SellerPolicy)]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateStore(Guid id, StoreDto storeDto)
    {
        Console.WriteLine("[PUT] StoreId: " + id);
        await _repository.UpdateStoreAsync(id, storeDto);
        return Ok();
    }
    
    
    [Authorize(Policy = Policies.SellerPolicy)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteStore(Guid id)
    {
        
        var store = await _repository.GetStoreAsync(id);
        

        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (currentUserId != store.OwnerId)
        {
            return Unauthorized();
        }


        await _repository.DeleteStoreAsync(id);
        return Ok();
    }
    
    
}
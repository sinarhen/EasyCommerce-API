using AutoMapper;
using ECommerce.Config;
using ECommerce.Data.Repositories.Store;
using ECommerce.Models.DTOs;
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

    [Authorize(Roles = UserRoles.Admin + "," + UserRoles.SuperAdmin)]
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
    
    [Authorize(Roles = UserRoles.Admin + "," + UserRoles.SuperAdmin + "," + UserRoles.Seller)]
    [HttpPost]
    public async Task<ActionResult> CreateStore(StoreDto storeDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await _repository.CreateStoreAsync(storeDto, userId);
        return Ok();
    }
    
    
    [Authorize(Roles = UserRoles.Admin + "," + UserRoles.SuperAdmin)]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateStore(Guid storeId, StoreDto storeDto)
    {
        
        await _repository.UpdateStoreAsync(storeId, storeDto);
        return Ok();
    }
    
    
    [Authorize(Roles = UserRoles.Admin + "," + UserRoles.SuperAdmin + "," + UserRoles.Seller)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteStore(Guid storeId)
    {
        var store = await _repository.GetStoreAsync(storeId);
        if (store.OwnerId != User.FindFirstValue(ClaimTypes.NameIdentifier))
        {
            return Unauthorized();
        }

        await _repository.DeleteStoreAsync(storeId);
        return Ok();
    }
    
    
}
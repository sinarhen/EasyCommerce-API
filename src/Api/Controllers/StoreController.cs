using AutoMapper;
using ECommerce.Config;
using ECommerce.Data.Repositories.Store;
using ECommerce.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        var store = _repository.GetStoreAsync(id);
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
        return Ok();
    }
    
    [Authorize(Roles = UserRoles.Admin + "," + UserRoles.SuperAdmin + "," + UserRoles.Seller)]
    [HttpPost]
    public async Task<ActionResult> CreateStore()
    {
        return Ok();
    }
    
    
    [Authorize(Roles = UserRoles.Admin + "," + UserRoles.SuperAdmin)]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateStore()
    {
        return Ok();
    }
    
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteStore()
    {
        return Ok();
    }
    
    
    
    
}
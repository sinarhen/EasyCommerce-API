using System.Security.Claims;
using AutoMapper;
using ECommerce.Config;
using ECommerce.Data.Repositories.Billboard;
using ECommerce.Models.DTOs.Billboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers;

[ApiController]
[Route("api/billboards")]
public class BillboardController : GenericController
{
    private readonly IBillboardRepository _repository;

    public BillboardController(IMapper mapper, IBillboardRepository repository) : base(mapper)
    {
        _repository = repository;
    }


    [HttpGet("{collectionId}")]
    public async Task<IActionResult> GetBillboardsForCollection(Guid collectionId)
    {
        try {
            var billboards = await _repository.GetBillboardsForCollectionAsync(collectionId);
            return Ok(_mapper.Map<List<BillboardDto>>(billboards));
        } 
        catch (UnauthorizedAccessException e) {
            return Unauthorized(e.Message);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e) {
            return StatusCode(500, e.Message);

        }
    }

    [HttpPost("{collectionId}")]
    [Authorize(Policy = Policies.SellerPolicy)]
    public async Task<IActionResult> CreateBillboardForCollection(Guid collectionId, [FromBody] CreateBillboardDto createBillboardDto)
    {
        try {
            var billboard = await _repository.CreateBillboardForCollectionAsync(collectionId, GetUserId(), createBillboardDto, IsAdmin());
            return Ok(_mapper.Map<BillboardDto>(billboard));
    
        } 
        catch (UnauthorizedAccessException e) {
            return Unauthorized(e.Message);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e) {
            return StatusCode(500, e.Message);

        }
    }

    [HttpPut("{billboardId}")]
    [Authorize(Policy = Policies.SellerPolicy)]
    public async Task<IActionResult> UpdateBillboard(Guid billboardId, [FromBody] UpdateBillboardDto updateBillboardDto)
    {
        try {
            var billboard = await _repository.UpdateBillboardAsync(updateBillboardDto, GetUserId(), billboardId, IsAdmin());
            return Ok(_mapper.Map<BillboardDto>(billboard));
    
        } 
        catch (UnauthorizedAccessException e) {
            return Unauthorized(e.Message);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e) {
            return StatusCode(500, e.Message);
        }
    }

    [HttpDelete("{billboardId}")]
    [Authorize(Policy = Policies.SellerPolicy)]
    public async Task<IActionResult> DeleteBillboard(Guid billboardId)
    {
        await _repository.DeleteBillboardAsync(billboardId, GetUserId(), IsAdmin());
        return Ok();
    }

}
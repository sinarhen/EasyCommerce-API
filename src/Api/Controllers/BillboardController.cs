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
        var billboards = await _repository.GetBillboardsForCollectionAsync(collectionId);
        return Ok(_mapper.Map<List<BillboardDto>>(billboards));
    }

    [HttpPost("{collectionId}")]
    [Authorize(Policy = Policies.SellerPolicy)]
    public async Task<IActionResult> CreateBillboardForCollection(Guid collectionId, [FromBody] CreateBillboardDto createBillboardDto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var billboard = await _repository.CreateBillboardForCollectionAsync(collectionId, userId, createBillboardDto);
        return Ok(_mapper.Map<BillboardDto>(billboard));
    }

    [HttpPut("{billboardId}")]
    [Authorize(Policy = Policies.SellerPolicy)]
    public async Task<IActionResult> UpdateBillboard(Guid billboardId, [FromBody] UpdateBillboardDto updateBillboardDto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var billboard = await _repository.UpdateBillboardAsync(updateBillboardDto, userId, billboardId);
        return Ok(_mapper.Map<BillboardDto>(billboard));
    }

    [HttpDelete("{billboardId}")]
    [Authorize(Policy = Policies.SellerPolicy)]
    public async Task<IActionResult> DeleteBillboard(Guid billboardId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        await _repository.DeleteBillboard(billboardId, userId);
        return Ok();
    }

}
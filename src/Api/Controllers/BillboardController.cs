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

    [HttpGet("{billboardId}")]
    public async Task<IActionResult> GetBillboard(Guid billboardId)
    {
        // TODO: Implement
        await Task.Delay(0);
        return Ok();
    }

    [HttpPost("{collectionId}")]
    [Authorize(Policy = Policies.SellerPolicy)]
    public async Task<IActionResult> CreateBillboardForCollection(Guid collectionId)
    {
        // TODO: Implement
        await Task.Delay(0);
        return Ok();
    }

    [HttpPut("{billboardId}")]
    [Authorize(Policy = Policies.SellerPolicy)]
    public async Task<IActionResult> UpdateBillboard(Guid billboardId)
    {
        // TODO: Implement
        await Task.Delay(0);
        return Ok();
    }

    [HttpDelete("{billboardId}")]
    [Authorize(Policy = Policies.SellerPolicy)]
    public async Task<IActionResult> DeleteBillboard(Guid billboardId)
    {

        await Task.Delay(0);
        return Ok();
    }

}
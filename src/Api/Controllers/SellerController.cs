using AutoMapper;
using ECommerce.Config;
using ECommerce.Data.Repositories.Seller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers;

[ApiController]
[Route("api/seller")]
[Authorize(Policy = Policies.SellerPolicy)]
public class SellerController : GenericController
{
    private readonly ISellerRepository _repository;

    public SellerController(IMapper mapper, ISellerRepository sellerRepository) : base(mapper)
    {
        _repository = sellerRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetSellerInfo()
    {
        var id = GetUserId();

        var info = await _repository.GetSellerInfo(id);

        return Ok(info);
    }
    
    
    [HttpGet("orders")]
    public async Task<IActionResult> GetOrders()
    {
        var id = GetUserId();

        var orders = await _repository.GetOrdersForSeller(id);

        return Ok(orders);
    }
}
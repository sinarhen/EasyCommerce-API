using AutoMapper;
using ECommerce.Config;
using ECommerce.Data.Repositories.Seller;
using ECommerce.Models.Enum;
using ECommerce.Services;
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
    
    [HttpPatch("orders/{orderId:guid}")]
    [ServiceFilter(typeof(ValidationService))]
    public async Task<IActionResult> UpdateOrderStatus(Guid orderId, [FromBody] UpdateOrderStatusDto status)
    {
        await _repository.UpdateOrderStatus(orderId, Enum.Parse<OrderItemStatus>(status.Status));
        return Ok("Order status updated");
    }
}
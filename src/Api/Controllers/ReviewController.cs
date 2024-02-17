using AutoMapper;
using ECommerce.Data.Repositories.Review;
using ECommerce.Models.DTOs.Review;
using ECommerce.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers;

[ApiController]
[Route("api/products/{productId}/reviews")]
public class ReviewController : GenericController
{
    private readonly IReviewRepository _repository;

    public ReviewController(IMapper mapper, IReviewRepository repository) : base(mapper)
    {
        _repository = repository;
    }


    [HttpPost]
    [Authorize]
    [ServiceFilter(typeof(ValidationService))]
    public async Task<IActionResult> CreateReviewForProduct(Guid productId, CreateReviewDto createReviewDto)
    {
        var res = await _repository.CreateReviewForProduct(productId, GetUserId(), createReviewDto);
        return Ok(res);
    }

    [HttpDelete("{reviewId:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteReview(Guid reviewId)
    {
        await _repository.DeleteReviewForCollectionAsync(reviewId, GetUserId());
        return Ok();
    }
}
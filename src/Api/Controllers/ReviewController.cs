using System.Security.Claims;
using AutoMapper;
using Data.Repositories.Review;
using ECommerce.Config;
using ECommerce.Data.Repositories.Billboard;
using ECommerce.Models.DTOs.Billboard;
using ECommerce.Models.DTOs.Review;
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


    // [HttpGet]
    // public async Task<IActionResult> GetReviewsForProduct(Guid productId)
    // {
    //     try {
    //         //TODO: Implement
    //         throw new NotImplementedException();
    //     } 
    //     catch (UnauthorizedAccessException e) {
    //         return Unauthorized(e.Message);
    //     }
    //     catch (ArgumentException e)
    //     {
    //         return BadRequest(e.Message);
    //     }
    //     catch (Exception e) {
    //         return StatusCode(500, e.Message);

    //     }
    // }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateReviewForProduct(Guid productId, CreateReviewDto createReviewDto)
    {
        try {
            var res = await _repository.CreateReviewForProduct(productId, GetUserId(), createReviewDto);
            return Ok(res);
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

    // [HttpPut("{reviewId}")]
    // public async Task<IActionResult> UpdateReview(Guid reviewId) // TODO: [FromBody] UpdateReviewDto updateReviewDto
    // {
    //     try {
    //         //TODO: Implement
    //         throw new NotImplementedException();
    //     } 
    //     catch (UnauthorizedAccessException e) {
    //         return Unauthorized(e.Message);
    //     }
    //     catch (ArgumentException e)
    //     {
    //         return BadRequest(e.Message);
    //     }
    //     catch (Exception e) {
    //         return StatusCode(500, e.Message);

    //     }
    
    // } Probably not needed

    [HttpDelete("{reviewId}")]
    [Authorize]
    public async Task<IActionResult> DeleteReview(Guid reviewId)
    {
        try {
            await _repository.DeleteReviewForCollectionAsync(reviewId, GetUserId());
            return Ok();
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

}
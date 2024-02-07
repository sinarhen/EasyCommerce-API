using System.Security.Claims;
using AutoMapper;
using Data.Repositories.Review;
using ECommerce.Config;
using ECommerce.Data.Repositories.Billboard;
using ECommerce.Models.DTOs.Billboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers;

[ApiController]
[Route("api/reviews")]
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

    [HttpPost("{productId}")]
    [Authorize(Policy = Policies.CustomerPolicy)]
    public async Task<IActionResult> CreateReviewForProduct(Guid productId) // TODO: [FromBody] CreateReviewDto createReviewDto
    {
        try {
            //TODO: Implement
            throw new NotImplementedException();
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
    [Authorize(Policy = Policies.CustomerPolicy)]
    public async Task<IActionResult> DeleteReview(Guid reviewId)
    {
        try {
            //TODO: Implement
            throw new NotImplementedException();
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
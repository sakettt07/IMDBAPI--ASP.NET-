using System;
using IMDBAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using IMDB.Domain.Models.RequestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using IMDBAPI.Services.CustomException;
using System.Threading.Tasks;

namespace IMDBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }
        [HttpGet("")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAsync()
        {
            var reviews = await _reviewService.GetAllAsync();
            return Ok(reviews);

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var review = await _reviewService.GetByIdAsync(id);
            return Ok(review);
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateAsync([FromBody] ReviewRequest reviewRequest)
        {
            var createdReview = await _reviewService.CreateAsync(reviewRequest);
            return Ok($"Review Created with Id: {createdReview}");
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] JsonPatchDocument<ReviewRequest> reviewRequest)
        {
            var updatedReview = await _reviewService.UpdateAsync(id, reviewRequest);
            return Ok(updatedReview);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _reviewService.DeleteAsync(id);
            return Ok("Review deleted successfully.");
        }
    }
}

using System.Threading.Tasks;
using System;
using IMDBAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using IMDB.Domain.Models.RequestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using IMDBAPI.Services.CustomException;
using Microsoft.AspNetCore.Http;

namespace IMDBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MoviesController:ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }
        [HttpGet("")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAsync([FromQuery] int? year)
        {
            var movies = await _movieService.GetAllAsync(year);
            return Ok(movies);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var movie = await _movieService.GetByIdAsync(id);
            return Ok(movie);
        }
        [HttpPost("")]
        public async Task<IActionResult> CreateAsync([FromBody] MovieRequest movieRequest)
        {
            var createdMovie = await _movieService.CreateAsync(movieRequest);
            return Ok($"New Movie created with Id :{createdMovie}");
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] JsonPatchDocument<MovieRequest> movieRequest)
        {
            var updated = await _movieService.UpdateAsync(id, movieRequest);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult >DeleteAsync(int id)
        {
            var result = await _movieService.DeleteAsync(id);
            return Ok("Movie deleted successfully.");
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadCoverImageAsync([FromForm] IFormFile file)
        {
            string imageUrl = await _movieService.UploadCoverImageAsync(file);
            return Ok(new { Message = "Cover Image uploaded successfully.", ImageUrl = imageUrl });
        }

    }
}


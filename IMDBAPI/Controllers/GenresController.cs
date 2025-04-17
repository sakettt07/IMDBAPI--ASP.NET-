using IMDB.Domain.Models.RequestModel;
using System.Threading.Tasks;
using System;
using IMDBAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using IMDBAPI.Services.CustomException;

namespace IMDBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GenresController:ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenresController(IGenreService genreService)
        {
            _genreService = genreService;
        }
        [HttpGet("")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAsync()
        {
            var genres = await _genreService.GetAllAsync();
            return Ok(genres);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var genre = await _genreService.GetByIdAsync(id);
            return Ok(genre);
        }
        [HttpPost("")]
        public async Task<IActionResult> CreateAsync([FromBody] GenreRequest genreRequest)
        {
            var genre = await _genreService.CreateAsync(genreRequest);
            return Ok(genre);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _genreService.DeleteAsync(id);
            return Ok("Genre deleted successfully.");
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] JsonPatchDocument<GenreRequest> genreRequest)
        {
            var genre = await _genreService.UpdateAsync(id, genreRequest);
            return Ok(genre);
        }
    }
}

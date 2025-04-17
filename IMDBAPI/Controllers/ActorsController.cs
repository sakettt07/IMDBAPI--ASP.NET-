using IMDB.Domain.Models.RequestModel;
using IMDBAPI.Services.CustomException;
using IMDBAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace IMDBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ActorsController : ControllerBase
    {
        private readonly IActorService _actorService;

        public ActorsController(IActorService actorService)
        {
            _actorService = actorService;
        }
        [HttpGet("")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAsync()
        {
            var actors = await _actorService.GetAllAsync();
            return Ok(actors);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var actor = await _actorService.GetByIdAsync(id);
            return Ok(actor);
        }
        [HttpPost("")]
        public async Task<IActionResult> CreateAsync([FromBody] ActorRequest actorRequest)
        {
            var actor = await _actorService.CreateAsync(actorRequest);
            return Ok($"New Actor Created with ID : {actor}");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _actorService.DeleteAsync(id);
            return Ok("Actor deleted successfully.");
        }
        [HttpPatch("{id}")]
            public async Task<IActionResult> UpdateAsync(int id, [FromBody] JsonPatchDocument<ActorRequest> ActorRequest)
        {
            var actor = await _actorService.UpdateAsync(id, ActorRequest);
            return Ok(actor);
        }
    }
}
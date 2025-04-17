using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IMDB.Domain.Models.RequestModel;
using IMDBAPI.Services.CustomException;
using IMDBAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace IMDBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProducersController:ControllerBase
    {
        private readonly IProducerService _producerService;

        public ProducersController(IProducerService producerService)
        {
            _producerService = producerService;
        }
        [HttpGet("")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAsync() {
            var producers = await _producerService.GetAllAsync();
            return Ok(producers);

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id) {
            var producer = await _producerService.GetByIdAsync(id);
            return Ok(producer);
        }
        [HttpPost("")]
        public async Task<IActionResult> CreateAsync([FromBody] ProducerRequest producerRequest) {
            var producer = await _producerService.CreateAsync(producerRequest);
            return Ok($"New Producer created with Id :{producer}");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id) {
            var result = await _producerService.DeleteAsync(id);
            return Ok("Producer deleted successfully.");
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] JsonPatchDocument<ProducerRequest> producerRequest) {
            var producer = await _producerService.UpdateAsync(id, producerRequest);
            return Ok(producer);
        }
    }
}

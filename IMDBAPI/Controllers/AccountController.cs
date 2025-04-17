using System;
using System.Threading.Tasks;
using IMDB.Domain.Models.RequestModel;
using IMDBAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IMDBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpPost("signup")]
        public async Task<IActionResult> SignUpAsync([FromBody] SignUpRequest signUpRequest)
        {
                var result = await _accountService.SignUpAsync(signUpRequest);
                return Ok("User registered successfully");
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest loginRequest)
        {

                var token = await _accountService.LoginAsync(loginRequest);
                return Ok(new { Token = token });

        }
    }
}

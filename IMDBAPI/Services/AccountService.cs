
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using System.Threading.Tasks;
using IMDB.Domain.Models.RequestModel;
using IMDB.Repository.Interfaces;
using IMDBAPI.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using IMDB.Domain.Models.DBModel;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;

namespace IMDBAPI.Services
{
    public class AccountService:IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IConfiguration _configuration;
        public AccountService(IAccountRepository accountRepository,IConfiguration configuration)
        {
            _accountRepository = accountRepository;
            _configuration = configuration;
        }

        public async Task<string> LoginAsync(LoginRequest loginRequest)
        {
            var user = await _accountRepository.GetUserByEmailAsync(loginRequest.Email);
            if (user == null || !ValidateUserCredentials(loginRequest.Password, user.Password, user.Key))
            {
                throw new UnauthorizedAccessException("Invalid Email or Password");
            }
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginRequest.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var authSigninKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWTkey:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWTkey:ValidIssuer"],
                audience: _configuration["JWTkey:ValidAudience"],
                expires: DateTime.Now.AddDays(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256Signature)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<bool> SignUpAsync(SignUpRequest signupRequest)
        {


            if (await _accountRepository.GetUserByEmailAsync(signupRequest.Email)!=null)
            {
                throw new InvalidOperationException("User already exists with this email.");
            }

            GetPasswordHash(signupRequest.Password, out string hashedPassword, out string uniqueKey);

            var user = new User
            {
                FirstName = signupRequest.FirstName,
                LastName = signupRequest.LastName,
                Email = signupRequest.Email,
                UserName = signupRequest.UserName,
                Password = hashedPassword,
                Key = uniqueKey
            };

            return await _accountRepository.SignUpAsync(user);
        }
        private void GetPasswordHash(string password, out string hashedPassword, out string uniqueKey)
        {
            using (var hmac = new HMACSHA256())
            {
                uniqueKey = Convert.ToBase64String(hmac.Key);
                hashedPassword = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
        }

        private bool ValidateUserCredentials(string password, string hashedPassword, string uniqueKey)
        {
            using (var hmac = new HMACSHA256(Convert.FromBase64String(uniqueKey)))
            {
                var checkedPassword = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
                return checkedPassword == hashedPassword;
            }
        }
    }
}

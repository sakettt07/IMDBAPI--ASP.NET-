using IMDB.Domain.Models.RequestModel;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace IMDBAPI.Services.Interfaces
{
    public interface IAccountService
    {
        Task<string> LoginAsync(LoginRequest loginRequest);
        Task<bool> SignUpAsync(SignUpRequest signupRequest);
    }
}

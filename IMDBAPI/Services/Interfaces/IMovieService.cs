using IMDB.Domain.Models.RequestModel;
using IMDB.Domain.Models.ResponseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IMDBAPI.Services.Interfaces
{
    public interface IMovieService
    {
        Task<List<MovieResponse>> GetAllAsync(int? year);
        Task<MovieResponse> GetByIdAsync(int id);
        Task<int> CreateAsync(MovieRequest movieRequest);
        Task<MovieResponse> UpdateAsync(int id, JsonPatchDocument<MovieRequest> movieRequest);
        Task<bool> DeleteAsync(int id);
        Task<string> UploadCoverImageAsync(IFormFile file);
    }
}

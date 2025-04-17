using IMDB.Domain.Models.RequestModel;
using IMDB.Domain.Models.ResponseModel;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IMDBAPI.Services.Interfaces
{
    public interface IGenreService
    {
        Task<List<GenreResponse>> GetAllAsync();
        Task<GenreResponse> GetByIdAsync(int id);
        Task<int> CreateAsync(GenreRequest genreRequest);
        Task<GenreResponse> UpdateAsync(int id, JsonPatchDocument<GenreRequest> genreRequest);
        Task<bool> DeleteAsync(int id);
        Task<List<int>> GetGenresFromMoviesAsync(int movieId);
    }
}

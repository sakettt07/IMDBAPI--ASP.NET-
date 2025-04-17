using System.Collections.Generic;
using System.Threading.Tasks;
using IMDB.Domain.Models.RequestModel;
using IMDB.Domain.Models.ResponseModel;
using Microsoft.AspNetCore.JsonPatch;

namespace IMDBAPI.Services.Interfaces
{
    public interface IActorService
    {
        Task<List<ActorResponse>> GetAllAsync();
        Task<ActorResponse> GetByIdAsync(int id);
        Task<int> CreateAsync(ActorRequest actorRequest);
        Task<ActorResponse> UpdateAsync(int id, JsonPatchDocument<ActorRequest> actorRequest);
        Task<bool> DeleteAsync(int id);
        Task<List<int>> GetActorsFromMoviesAsync(int movieId);
    }
}

using IMDB.Domain.Models.RequestModel;
using IMDB.Domain.Models.ResponseModel;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IMDBAPI.Services.Interfaces
{
    public interface IProducerService
    {
        Task<List<ProducerResponse>> GetAllAsync();
        Task<ProducerResponse> GetByIdAsync(int id);
        Task<int> CreateAsync(ProducerRequest producerRequest);
        Task<ProducerResponse> UpdateAsync(int id, JsonPatchDocument<ProducerRequest> producerRequest);
        Task<bool> DeleteAsync(int id);
    }
}

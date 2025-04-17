using IMDB.Domain.Models.RequestModel;
using IMDB.Domain.Models.ResponseModel;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IMDBAPI.Services.Interfaces
{
    public interface IReviewService
    {
        Task<List<ReviewResponse>> GetAllAsync();
        Task<ReviewResponse> GetByIdAsync(int id);
        Task<int> CreateAsync(ReviewRequest reviewRequest);
        Task<ReviewResponse> UpdateAsync(int id, JsonPatchDocument<ReviewRequest> reviewRequest);
       Task<bool> DeleteAsync(int id);
    }
}

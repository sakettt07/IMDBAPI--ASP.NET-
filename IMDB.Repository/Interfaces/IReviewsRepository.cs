using System.Collections.Generic;
using System.Threading.Tasks;
using IMDB.Domain.Models.DBModel;

namespace IMDB.Repository.Interfaces
{
    public interface IReviewsRepository
    {
        Task<List<Review>> GetAllAsync();
        Task<Review> GetByIdAsync(int id);
        Task<int> CreateAsync(Review review);
        Task<Review> UpdateAsync(Review review);
        Task<bool> DeleteAsync(int id);
    }
}

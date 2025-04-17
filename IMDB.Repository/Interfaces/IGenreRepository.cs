using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMDB.Domain.Models.DBModel;

namespace IMDB.Repository.Interfaces
{
    public interface IGenreRepository
    {
        Task<List<Genre>> GetAllAsync();
        Task<Genre> GetByIdAsync(int id);
        Task<int> CreateAsync(Genre genre);
        Task<Genre> UpdateAsync(Genre genre);
        Task<bool> DeleteAsync(int id);
        Task<List<int>> GetGenresFromMoviesAsync(int id);
    }
}

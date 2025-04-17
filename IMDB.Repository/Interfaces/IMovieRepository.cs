
using System.Collections.Generic;
using System.Threading.Tasks;
using IMDB.Domain.Models.DBModel;

namespace IMDB.Repository.Interfaces
{
    public interface IMovieRepository
    {
        Task<List<Movie>> GetAllAsync();
        Task<Movie> GetByIdAsync(int id);
        Task<int> CreateAsync(Movie movies,List<Actor> actors,List<Genre>genres);
        Task<Movie> UpdateAsync(Movie movies, List<Actor> actors, List<Genre> genres);
        Task<bool> DeleteAsync(int id);
    }
}

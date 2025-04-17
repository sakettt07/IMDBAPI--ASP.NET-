using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMDB.Domain.Models.DBModel;

namespace IMDB.Repository.Interfaces
{
    public interface IActorRepository
    {
        Task<List<Actor>> GetAllAsync();
        Task<Actor> GetByIdAsync(int id);
        Task<int> CreateAsync(Actor actor);
        Task<Actor> UpdateAsync(Actor actor);
        Task<bool> DeleteAsync(int id);
        Task<List<int>> GetActorsFromMovies(int id);
    }
}

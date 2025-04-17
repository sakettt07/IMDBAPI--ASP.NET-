using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMDB.Domain.Models.DBModel;

namespace IMDB.Repository.Interfaces
{
    public interface IProducerRepository
    {
        Task<List<Producer>> GetAllAsync();
        Task<Producer> GetByIdAsync(int id);
        Task<int> CreateAsync(Producer producer);
        Task<Producer> UpdateAsync(Producer producer);
        Task<bool> DeleteAsync(int id);
    }
}

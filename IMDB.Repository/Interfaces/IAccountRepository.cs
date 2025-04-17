using System.Threading.Tasks;
using IMDB.Domain.Models.DBModel;


namespace IMDB.Repository.Interfaces
{
    public interface IAccountRepository
    {
        Task<bool> SignUpAsync(User user);
        Task<User> GetUserByEmailAsync(string email);
    }
}

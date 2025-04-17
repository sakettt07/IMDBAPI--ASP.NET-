
using System.Threading.Tasks;
using IMDB.Domain.Models.DBModel;
using IMDB.Repository.DBConnection;
using IMDB.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace IMDB.Repository
{
    public class AccountRepository :BaseRepository<User>, IAccountRepository
    {
        public AccountRepository(IOptions<IMDBdbContext>options) : base(options.Value.IMDB) { }

        public async Task<bool> SignUpAsync(User user)
        {
            var sql = @"
    INSERT INTO Users (
        FirstName, 
        LastName, 
        Email, 
        UserName, 
        Password, 
        [Key]
    ) 
    VALUES (
        @FirstName, 
        @LastName, 
        @Email, 
        @UserName, 
        @Password, 
        @Key
    )";

            return await CreateAsync(sql, user) > 0;

        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            var query = @"
    SELECT 
        [Key], 
        Password, 
        UserName 
    FROM Users 
    WHERE Email = @Email";

            return await GetByIdAsync(query, new { Email = email });

        }

    }
}

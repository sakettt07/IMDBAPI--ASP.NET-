using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using IMDB.Domain.Models.DBModel;
using IMDB.Repository.DBConnection;
using IMDB.Repository.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace IMDB.Repository
{
    public class GenreRepository:BaseRepository<Genre>,IGenreRepository
    {
        public GenreRepository(IOptions<IMDBdbContext> options) : base(options.Value.IMDB)
        {

        }
        public async Task<int> CreateAsync(Genre genre)
        {
            string sql = @"
                INSERT INTO Genres (Name) 
                VALUES (@Name); 
                SELECT CAST(SCOPE_IDENTITY() AS INT)";

            return await CreateAsync(sql, new { Name = genre.Name });
        }

        public async Task<bool> DeleteAsync(int id)
        {
            string sql = @"
            DELETE FROM GenresMovies WHERE GenreId = @Id;
            DELETE FROM Genres WHERE Id = @Id";

            return await DeleteAsync(sql, new { Id = id });
        }

        public async Task<List<Genre>> GetAllAsync()
        {
            string sql = @"
                SELECT
                [Id],
                [Name]
                FROM Genres";

            return await GetAllAsync(sql) as List<Genre>;
        }

        public async Task<Genre> GetByIdAsync(int id)
        {
            string sql = @"
                SELECT 
                [Id],
                [Name]
                FROM Genres 
                WHERE Id = @Id";

            return await GetByIdAsync(sql, new { Id = id });
        }

        public async Task<Genre> UpdateAsync(Genre genre)
        {
            string sql = @"
                UPDATE Genres 
                SET Name = @Name 
                WHERE Id = @Id";

            bool isUpdated = await UpdateAsync(sql, new { Name=genre.Name, Id=genre.Id });

            if (!isUpdated)
            {
                throw new Exception($"Update failed for Genre with ID {genre.Id}");
            }
            return await GetByIdAsync(genre.Id);
        }
        public async Task<List<int>> GetGenresFromMoviesAsync(int id)
        {
            string sql = @"
                SELECT GenreId
                FROM GenresMovies
                WHERE MovieId = @Id";
            using (var connection = new SqlConnection(_connectionString))
            {
                return (List<int>)await connection.QueryAsync<int>(sql, new { Id = id });
            }
        }
    }
}

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
    public class MovieRepository :BaseRepository<Movie>, IMovieRepository
    {
        public MovieRepository(IOptions<IMDBdbContext> options):base(options.Value.IMDB)
        {
        }
        public async Task<int> CreateAsync(Movie movie, List<Actor> actors, List<Genre> genres)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var param = new DynamicParameters();
                param.Add("@Name", movie.Name);
                param.Add("@YearOfRelease", movie.YearOfRelease);
                param.Add("@Plot", movie.Plot);
                param.Add("@CoverImage", movie.CoverImage);
                param.Add("@ProducerId", movie.ProducerId);
                param.Add("@ActorIds", string.Join(',', actors.Select(g => g.Id)));
                param.Add("@GenreIds", string.Join(',',genres.Select(g => g.Id)));
                param.Add("@MovieId", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                await connection.ExecuteAsync("usp_InsertMovie", param, commandType: System.Data.CommandType.StoredProcedure);
                return param.Get<int>("@MovieId");
            }
        }
        public async Task<List<Movie>> GetAllAsync()
        {
            string sql = @"
    SELECT *
    FROM Movies 
    ";
            return (await GetAllAsync(sql)).ToList();


        }
        public async Task<Movie> GetByIdAsync(int id)
        {
            string sql = @"
    SELECT 
* FROM Movies 
    WHERE Id = @Id
    ";
            return await GetByIdAsync(sql, new { Id = id });
        }
        public async Task<Movie> UpdateAsync(Movie movie, List<Actor> actors, List<Genre> genres)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var param = new DynamicParameters();
                param.Add("@MovieId", movie.Id);
                param.Add("@Name", movie.Name);
                param.Add("@YearOfRelease", movie.YearOfRelease);
                param.Add("@Plot", movie.Plot);
                param.Add("@CoverImage", movie.CoverImage);
                param.Add("@ProducerId", movie.ProducerId);
                param.Add("@ActorIds", actors != null ? string.Join(',', actors.Select(a => a.Id)) : (object)DBNull.Value);
                param.Add("@GenreIds", genres != null ? string.Join(',', genres.Select(g => g.Id)) : (object)DBNull.Value);
                param.Add("@IsUpdated", dbType: System.Data.DbType.Boolean, direction: System.Data.ParameterDirection.Output);

                await connection.ExecuteAsync("usp_UpdateMovie", param, commandType: System.Data.CommandType.StoredProcedure);

                bool isUpdated = param.Get<bool>("@IsUpdated");
                if (!isUpdated)
                {
                    throw new Exception($"Update failed for Movie with ID {movie.Id}");
                }
                return await GetByIdAsync(movie.Id);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            string sql = @"
    DELETE FROM ActorsMovies WHERE MovieId = @Id;
    DELETE FROM GenresMovies WHERE MovieId = @Id;
    DELETE FROM Movies WHERE Id = @Id;
";
            return await DeleteAsync(sql, new { Id = id });
        }
    }
}

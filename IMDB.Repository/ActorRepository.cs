
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using IMDB.Domain.Models.DBModel;
using IMDB.Repository.DBConnection;
using IMDB.Repository.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace IMDB.Repository
{
    public class ActorRepository:BaseRepository<Actor>,IActorRepository
    {
        public ActorRepository(IOptions<IMDBdbContext> options) : base(options.Value.IMDB)
        {

        }
        public async Task<List<Actor>> GetAllAsync()
        {
            string sql = @"
                SELECT
                [Id],
                [Name],
                [DOB],
                [Gender],
                [Bio]
                FROM Actors";

            return await GetAllAsync(sql) as List<Actor>;
        }
        public async Task<Actor> GetByIdAsync(int id)
        {
            string sql = @"
                SELECT
                [Id],
                [Name],
                [DOB],
                [Gender],
                [Bio]
                FROM Actors 
                WHERE Id = @Id";

            return await GetByIdAsync(sql, new { Id = id });
        }
        public async Task<int> CreateAsync(Actor actor)
        {
            string sql = @"
                INSERT INTO Actors (Name, DOB, Bio, Gender) 
                VALUES (@Name, @DOB, @Bio, @Gender); 
                
                SELECT CAST(SCOPE_IDENTITY() AS INT)";

            return await CreateAsync(sql, new
            {
                Name = actor.Name,
                DOB = actor.DOB,
                Bio = actor.Bio,
                Gender = actor.Gender
            });
        }
        public async Task<Actor> UpdateAsync(Actor actor)
        {
            string sql = @"
                UPDATE Actors 
                SET Name = @Name, 
                    Bio = @Bio, 
                    DOB = @DOB, 
                    Gender = @Gender 
                WHERE Id = @Id";

            bool isUpdated = await UpdateAsync(sql, new
            {
                Name = actor.Name,
                Bio = actor.Bio,
                DOB = actor.DOB,
                Gender = actor.Gender,
                Id = actor.Id
            });
            if (!isUpdated)
            {
                throw new Exception($"Update failed for Actor with ID {actor.Id}");
            }
            return await GetByIdAsync(actor.Id);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            string sql = @"
                DELETE FROM ActorsMovies 
                WHERE ActorId = @Id;
                DELETE FROM Actors 
                WHERE Id = @Id";
            return await DeleteAsync(sql, new { Id = id });
        }

        public async Task<List<int>> GetActorsFromMovies(int id)
        {
            string sql = @"
                SELECT ActorId
                FROM ActorsMovies
                WHERE MovieId = @Id";
            using (var connection = new SqlConnection(_connectionString))
            {
                return (List<int>)await connection.QueryAsync<int>(sql, new { Id = id });
            }
        }
    }
}

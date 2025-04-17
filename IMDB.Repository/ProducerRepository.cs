using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IMDB.Domain.Models.DBModel;
using IMDB.Repository.DBConnection;
using IMDB.Repository.Interfaces;
using Microsoft.Extensions.Options;

namespace IMDB.Repository
{
    public class ProducerRepository:BaseRepository<Producer>,IProducerRepository
    {
        public ProducerRepository(IOptions<IMDBdbContext> options):base(options.Value.IMDB)
        {
            
        }
        public async Task<List<Producer>> GetAllAsync()
        {
            string sql = @"
            SELECT 
                [Id],
                [Name],
                [DOB],
                [Gender],
                [Bio]
    FROM [Producers]";
            return await GetAllAsync(sql) as List<Producer>;
        }

        public async Task<Producer> GetByIdAsync(int id)
        {
            string sql = @"
            SELECT 
                [Id],
                [Name],
                [DOB],
                [Gender],
                [Bio]
    FROM [Producers] 
    WHERE Id = @Id";

            return await GetByIdAsync(sql, new { Id = id });
        }

        public async Task<int> CreateAsync(Producer producer)
        {
            string sql = @"
    INSERT INTO [Producers] (Name, DOB, Bio, Gender) 
    VALUES (@Name, @DOB, @Bio, @Gender); 
    
    SELECT CAST(SCOPE_IDENTITY() AS INT);";

            return await CreateAsync(sql, new
            {
                Name = producer.Name,
                DOB = producer.DOB,
                Bio = producer.Bio,
                Gender = producer.Gender
            });
        }

        public async Task<Producer> UpdateAsync(Producer producer)
        {
            string sql = @"
    UPDATE [Producers] 
    SET 
        Name = @Name, 
        Bio = @Bio, 
        DOB = @DOB, 
        Gender = @Gender 
    WHERE Id = @Id";

            bool isUpdated = await UpdateAsync(sql, new
            {
                Name = producer.Name,
                Bio = producer.Bio ?? (object)DBNull.Value,
                DOB = producer.DOB,
                Gender = producer.Gender ?? (object)DBNull.Value,
                Id = producer.Id
            });

            if (!isUpdated)
            {
                throw new Exception($"Update failed for Producer with ID {producer.Id}");
            }
            return await GetByIdAsync(producer.Id);
        }

        public async Task<bool>DeleteAsync(int id)
        {
            string sql = @"
    DELETE FROM [Movies] WHERE ProducerId = @Id;
    DELETE FROM [Producers] WHERE Id = @Id";
            return await DeleteAsync(sql, new { Id = id });
        }

    }
}

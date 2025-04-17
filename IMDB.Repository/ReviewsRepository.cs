using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using IMDB.Domain.Models.DBModel;
using IMDB.Repository.DBConnection;
using IMDB.Repository.Interfaces;
using Microsoft.Extensions.Options;

namespace IMDB.Repository
{
    public class ReviewsRepository :BaseRepository<Review>, IReviewsRepository
    {
        public ReviewsRepository(IOptions<IMDBdbContext> options):base(options.Value.IMDB)
        {
            
        }
        public async Task<int> CreateAsync(Review review)
        {
            string sql = @"
    INSERT INTO Reviews (Message, MovieId) 
    VALUES (@Message, @MovieId);

    SELECT CAST(SCOPE_IDENTITY() AS INT);
";

            return await CreateAsync(sql, new { Message = review.Message, MovieId = review.MovieId });

        }

        public async Task<bool> DeleteAsync(int id)
        {
            string sql = @"
    DELETE FROM Reviews 
    WHERE Id = @Id;
";

            return await DeleteAsync(sql, new { Id = id });
        }

        public async Task<List<Review>> GetAllAsync()
        {
            string sql = @"
    SELECT 
        [Id], 
        [Message], 
        [MovieId] 
    FROM Reviews
";

            return await GetAllAsync(sql) as List<Review>;
        }

        public async Task<Review> GetByIdAsync(int id)
        {
            string sql = @"
    SELECT 
        [Id], 
        [Message], 
        [MovieId] 
    FROM Reviews 
    WHERE Id = @Id
";

            return await GetByIdAsync(sql, new { Id = id });
        }

        public async Task<Review> UpdateAsync(Review review)
        {
            string sql = @"
    UPDATE Reviews 
    SET 
        Message = @Message, 
        MovieId = @MovieId 
    WHERE 
        Id = @Id";

            bool isUpdated = await UpdateAsync(sql, new { review.Message, review.MovieId, review.Id });

            if (!isUpdated)
            {
                throw new Exception($"Update failed for Review with ID {review.Id}");
            }
            return await GetByIdAsync(review.Id);
        }
    }
}

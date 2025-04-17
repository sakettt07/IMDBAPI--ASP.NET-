using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IMDB.Repository
{
    public class BaseRepository<T> where T : class
    {
        protected readonly string _connectionString;

        public BaseRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<int> CreateAsync(string sql, object param)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                return await db.ExecuteScalarAsync<int>(sql, param);
            }
        }
        public async Task<bool> UpdateAsync(string sql, object param)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                return await db.ExecuteAsync(sql, param) > 0;
            }
        }
        public async Task<bool> DeleteAsync(string sql, object param)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                return await db.ExecuteAsync(sql, param) > 0;
            }
        }
        public async Task<T> GetByIdAsync(string sql, object param)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                return await db.QueryFirstOrDefaultAsync<T>(sql, param);
            }
        }
        public async Task<IEnumerable<T>> GetAllAsync(string sql)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                return await db.QueryAsync<T>(sql);
            }
        }
    }
}

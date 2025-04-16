using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using BookStoreAPI.Models;
using Microsoft.Extensions.Configuration;
using Dapper;

namespace BookStoreAPI.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly string _connectionString;

        public AuthorRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Author>> GetAllAuthorsAsync()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return await db.QueryAsync<Author>("spGetAllAuthors", commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Author> GetAuthorByIdAsync(int authorId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@AuthorId", authorId);

                return await db.QueryFirstOrDefaultAsync<Author>("spGetAuthorById", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> CreateAuthorAsync(Author author)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Name", author.Name);
                parameters.Add("@Bio", author.Bio);
                parameters.Add("@AuthorId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await db.ExecuteAsync("spInsertAuthor", parameters, commandType: CommandType.StoredProcedure);

                return parameters.Get<int>("@AuthorId");
            }
        }

        public async Task<bool> UpdateAuthorAsync(Author author)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@AuthorId", author.AuthorId);
                parameters.Add("@Name", author.Name);
                parameters.Add("@Bio", author.Bio);

                var affected = await db.ExecuteAsync("spUpdateAuthor", parameters, commandType: CommandType.StoredProcedure);

                return affected > 0;
            }
        }

        public async Task<bool> DeleteAuthorAsync(int authorId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@AuthorId", authorId);

                var affected = await db.ExecuteAsync("spDeleteAuthor", parameters, commandType: CommandType.StoredProcedure);

                return affected > 0;
            }
        }
    }
}
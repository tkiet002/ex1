using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using System.Linq;
using BookStoreAPI.Models;
using Microsoft.Extensions.Configuration;
using Dapper;

namespace BookStoreAPI.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly string _connectionString;

        public BookRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return await db.QueryAsync<Book, Author, Book>(
                    "spGetAllBooks",
                    (book, author) => {
                        book.Author = author;
                        return book;
                    },
                    splitOn: "AuthorId",
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Book> GetBookByIdAsync(int bookId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@BookId", bookId);

                var books = await db.QueryAsync<Book, Author, Book>(
                    "spGetBookById",
                    (book, author) => {
                        book.Author = author;
                        return book;
                    },
                    parameters,
                    splitOn: "AuthorId",
                    commandType: CommandType.StoredProcedure);

                return books.FirstOrDefault();
            }
        }

        public async Task<int> CreateBookAsync(Book book)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Title", book.Title);
                parameters.Add("@Price", book.Price);
                parameters.Add("@AuthorId", book.AuthorId);
                parameters.Add("@PublishedDate", book.PublishedDate);
                parameters.Add("@BookId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await db.ExecuteAsync("spInsertBook", parameters, commandType: CommandType.StoredProcedure);

                return parameters.Get<int>("@BookId");
            }
        }

        public async Task<bool> UpdateBookAsync(Book book)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@BookId", book.BookId);
                parameters.Add("@Title", book.Title);
                parameters.Add("@Price", book.Price);
                parameters.Add("@AuthorId", book.AuthorId);
                parameters.Add("@PublishedDate", book.PublishedDate);

                var affected = await db.ExecuteAsync("spUpdateBook", parameters, commandType: CommandType.StoredProcedure);

                return affected > 0;
            }
        }

        public async Task<bool> DeleteBookAsync(int bookId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@BookId", bookId);

                var affected = await db.ExecuteAsync("spDeleteBook", parameters, commandType: CommandType.StoredProcedure);

                return affected > 0;
            }
        }

        public async Task<IEnumerable<Book>> GetBooksByFiltersAsync(
            string searchKey, 
            int? authorId, 
            DateTime? fromPublishedDate, 
            DateTime? toPublishedDate, 
            int pageSize, 
            int pageIndex)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@SearchKey", searchKey);
                parameters.Add("@AuthorId", authorId);
                parameters.Add("@FromPublishedDate", fromPublishedDate);
                parameters.Add("@ToPublishedDate", toPublishedDate);
                parameters.Add("@PageSize", pageSize);
                parameters.Add("@PageIndex", pageIndex);

                return await db.QueryAsync<Book, Author, Book>(
                    "spGetBooksByFilters",
                    (book, author) => {
                        book.Author = author;
                        return book;
                    },
                    parameters,
                    splitOn: "AuthorId",
                    commandType: CommandType.StoredProcedure);
            }
        }
    }
}
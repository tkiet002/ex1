using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookStoreAPI.Models;

namespace BookStoreAPI.Repositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book> GetBookByIdAsync(int bookId);
        Task<int> CreateBookAsync(Book book);
        Task<bool> UpdateBookAsync(Book book);
        Task<bool> DeleteBookAsync(int bookId);
        Task<IEnumerable<Book>> GetBooksByFiltersAsync(string searchKey, int? authorId, DateTime? fromPublishedDate, DateTime? toPublishedDate, int pageSize, int pageIndex);
    }
}
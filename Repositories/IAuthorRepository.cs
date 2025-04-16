using System.Collections.Generic;
using System.Threading.Tasks;
using BookStoreAPI.Models;

namespace BookStoreAPI.Repositories
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<Author>> GetAllAuthorsAsync();
        Task<Author> GetAuthorByIdAsync(int authorId);
        Task<int> CreateAuthorAsync(Author author);
        Task<bool> UpdateAuthorAsync(Author author);
        Task<bool> DeleteAuthorAsync(int authorId);
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookStoreAPI.Models;
using BookStoreAPI.Repositories;

namespace BookStoreAPI.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet("fetch")]
        public async Task<ActionResult<IEnumerable<Book>>> GetAllBooks()
        {
            try
            {
                var books = await _bookRepository.GetAllBooksAsync();
                return Ok(books);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("fetch/{bookId}")]
        public async Task<ActionResult<Book>> GetBookById(int bookId)
        {
            try
            {
                var book = await _bookRepository.GetBookByIdAsync(bookId);

                if (book == null)
                    return NotFound($"Book with ID {bookId} not found");

                return Ok(book);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("insert")]
        public async Task<ActionResult<int>> CreateBook([FromBody] Book book)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var newBookId = await _bookRepository.CreateBookAsync(book);
                return Ok(newBookId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult> UpdateBook([FromBody] Book book)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var success = await _bookRepository.UpdateBookAsync(book);

                if (!success)
                    return NotFound($"Book with ID {book.BookId} not found");

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{bookId}")]
        public async Task<ActionResult> DeleteBook(int bookId)
        {
            try
            {
                var success = await _bookRepository.DeleteBookAsync(bookId);

                if (!success)
                    return NotFound($"Book with ID {bookId} not found");

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
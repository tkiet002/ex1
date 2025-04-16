using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookStoreAPI.Models;
using BookStoreAPI.Repositories;

namespace BookStoreAPI.Controllers
{
    [Route("api/authors")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorsController(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        [HttpGet("fetch")]
        public async Task<ActionResult<IEnumerable<Author>>> GetAllAuthors()
        {
            try
            {
                var authors = await _authorRepository.GetAllAuthorsAsync();
                return Ok(authors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("fetch/{authorId}")]
        public async Task<ActionResult<Author>> GetAuthorById(int authorId)
        {
            try
            {
                var author = await _authorRepository.GetAuthorByIdAsync(authorId);

                if (author == null)
                    return NotFound($"Author with ID {authorId} not found");

                return Ok(author);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("insert")]
        public async Task<ActionResult<int>> CreateAuthor([FromBody] Author author)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var newAuthorId = await _authorRepository.CreateAuthorAsync(author);
                return Ok(newAuthorId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult> UpdateAuthor([FromBody] Author author)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var success = await _authorRepository.UpdateAuthorAsync(author);

                if (!success)
                    return NotFound($"Author with ID {author.AuthorId} not found");

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{authorId}")]
        public async Task<ActionResult> DeleteAuthor(int authorId)
        {
            try
            {
                var success = await _authorRepository.DeleteAuthorAsync(authorId);

                if (!success)
                    return NotFound($"Author with ID {authorId} not found");

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
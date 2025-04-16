using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookStoreAPI.Models;
using BookStoreAPI.Repositories;

namespace BookStoreAPI.Controllers
{
    [Route("api/reports")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public ReportsController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet("book")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooksByFilters(
            [FromQuery] string searchKey = "",
            [FromQuery] int? authorId = null,
            [FromQuery] DateTime? fromPublishedDate = null,
            [FromQuery] DateTime? toPublishedDate = null,
            [FromQuery] int pageSize = 10,
            [FromQuery] int pageIndex = 0)
        {
            try
            {
                var books = await _bookRepository.GetBooksByFiltersAsync(
                    searchKey,
                    authorId,
                    fromPublishedDate,
                    toPublishedDate,
                    pageSize,
                    pageIndex);

                return Ok(books);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
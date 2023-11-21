using Library.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using Library.Models;

using System.Net;
using Library.Interfaces;

namespace Library.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController: ControllerBase
    {
        
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet("books/{id}")]
        public IActionResult GetBookById(int id)
        {
            var book = _bookService.GetBookById(id);
         
            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);

        }

        [HttpGet("filter-books")]
        public IActionResult GetBooksByFilter(string? authorName, string? title, bool? isAvailable)
        {
            var filtresbooks = _bookService.GetBooksByFilter(authorName, title, isAvailable);
            return Ok(filtresbooks);
        }

        [HttpPost("books/{id}/pick-up")]
        public IActionResult BorrowBook(int id)
        {
            var result = _bookService.BorrowBook(id);

            if (result == "Book not found.")
            {
                return NotFound(result);
            }
            else if (result == "The book has already been issued.")
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("books/{id}/return")]
        public IActionResult ReturnBook(int id)
        {
            var result = _bookService.ReturnBook(id);

            if (result == "Book not found.")
            {
                return NotFound(result);
            }
            else if (result == "Book is already available.")
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

    }
}

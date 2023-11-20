using Library.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using Library.Models;

namespace Library.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController: ControllerBase
    {
        private readonly LibraryDbContext _context;

        public BookController(LibraryDbContext context)
        {
            _context = context;
        }

        [HttpGet("All-books")]
        public IActionResult GetAllBooksAndAuthors()
        {
            var booksAndAuthors = _context.Books
                .Include(b => b.Authors) 
                .Select(b => new
                {
                    BookId = b.Id,
                    Title = b.Title,
                    IsAvailable = b.IsAvailable,
                    Authors = b.Authors.Select(a => new { AuthorId = a.Author.Id, AuthorName = a.Author.Name })
                })
                .ToList();

            return Ok(booksAndAuthors);
        }

        [HttpGet("books/{id}")]
        public IActionResult GetBookById(int id)
        {
            var book = _context.Books
                      .Include(b => b.Authors)
                      .FirstOrDefault(b => b.Id == id);

            if (book == null)
            {
                return NotFound(); 
            }

            return Ok(book);
        }

        
        [HttpGet("books filtres")]
        public IActionResult GetBooksByFilter(string? authorName, string? title, bool? isAvailable)
        {
            IQueryable<Book> query = _context.Books.Include(b => b.Authors);

            // Фильтрация по имени автора
            if (!string.IsNullOrEmpty(authorName))
            {
                query = query.Where(b => b.Authors.Any(a => a.Author.Name.Contains(authorName)));
            }

            // Фильтрация по названию книги
            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(b => b.Title.Contains(title));
            }

            // Фильтрация по наличию книги
            if (isAvailable.HasValue)
            {
                query = query.Where(b => b.IsAvailable == isAvailable.Value);
            }

            var books = query.ToList();

            

            return Ok(books);
        }

        [HttpPost("books/{id}/pick up")]
        public IActionResult BorrowBook(int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id);

            if (book == null)
            {
                return NotFound(); 
            }

            if (!book.IsAvailable)
            {
                return BadRequest("The book has already been issued."); // Книга уже взята
            }

            // Обновляем статус на "взято"
            book.IsAvailable = false;
            _context.SaveChanges();

            return Ok("The book has been issued.");
        }

        [HttpPost("books/{id}/return")]
        public IActionResult ReturnBook(int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id);

            if (book == null)
            {
                return NotFound(); 
            }

            if (book.IsAvailable)
            {
                return BadRequest("Book is already available."); // Книга уже возвращена
            }

            // Обновляем статус на "в наличии"
            book.IsAvailable = true;
            _context.SaveChanges();

            return Ok("Book returned successfully.");
        }
    }
}

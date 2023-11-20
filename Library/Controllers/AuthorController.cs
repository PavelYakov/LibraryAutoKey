using Library.Context;
using Library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
   
    public class AuthorController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public AuthorController(LibraryDbContext context)
        {
            _context = context;
        }

        [HttpGet("All-authors")]
        public IActionResult GetAllBooksAndAuthors()
        {
            var Authors = _context.Authors
                 
                .ToList();

            return Ok(Authors);
        }

        [HttpGet("author/{id}")]
        public IActionResult GetAuthorById(int id)
        {
            var author = _context.Authors
                      .Where(a => a.Id == id)
                      .FirstOrDefault();

            if (author == null)
            {
                return NotFound(); 
            }

            return Ok(author);

           
        }

        [HttpGet("authors filters")]
        public IActionResult GetAuthorsByFilter(string? title, bool? isAvailable)
        {
            IQueryable<Author> query = _context.Authors.Include(a => a.Books);

            // Фильтрация по названию книги
            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(a => a.Books.Any(ba => ba.Book.Title.Contains(title)));
            }

            // Фильтрация по наличию книги
            if (isAvailable.HasValue)
            {
                query = query.Where(a => a.Books.Any(b => b.Book.IsAvailable == isAvailable.Value));
            }

            var authors = query.ToList();

            return Ok(authors);
        }







    }
}

using Library.Context;
using Library.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Services
{
    public class AuthorService:IAuthorService
    {
        private readonly LibraryDbContext _context;
        public AuthorService(LibraryDbContext context)
        {
            _context = context;
        }
        public List<Author> GetAllAuthors() 
        {
            var Authors = _context.Authors
                      .ToList();
            return Authors;
        }
        public Author GetAuthorById(int id) 
        {
            return _context.Authors
                  .FirstOrDefault(b => b.Id == id);
        }
        public List<Author> GetAuthorsByFilter(string? title, bool? isAvailable) 
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

            return authors;

        }
    }
}

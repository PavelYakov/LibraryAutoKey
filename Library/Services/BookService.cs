using Library.Context;
using Library.Interfaces;
using Library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Services
{
    public class BookService : IBookService
    {
        private readonly LibraryDbContext _context;
        public BookService(LibraryDbContext context)
        {
            _context = context;
        }
        public Book GetBookById(int id)
        {
            return _context.Books
                .Include(b => b.Authors)
                .FirstOrDefault(b => b.Id == id);
        }
        public List<Book> GetBooksByFilter(string? authorName, string? title, bool? isAvailable)
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

            return books;
        }

        public string BorrowBook(int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id);

            if (book == null)
            {
                return "Book not found.";
            }

            if (!book.IsAvailable)
            {
                return "The book has already been issued.";
            }

            // Обновляем статус на "взято"
            book.IsAvailable = false;
            _context.SaveChanges();

            
            return "The book has been issued.";
        }

        public string ReturnBook(int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id);

            if (book == null)
            {
                return "Book not found.";
            }

            if (book.IsAvailable)
            {
                return "Book is already available.";
            }

            // Обновляем статус на "в наличии"
            book.IsAvailable = true;
            _context.SaveChanges();

            return "Book returned successfully.";
        }

    }
}
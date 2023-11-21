using Library.Models;
using Microsoft.AspNetCore.Mvc;

namespace Library.Interfaces
{
    public interface IBookService
    {
        Book GetBookById(int id);
        List<Book> GetBooksByFilter(string? authorName, string? title, bool? isAvailable);

        string BorrowBook(int id);

        string ReturnBook(int id);

    }
}

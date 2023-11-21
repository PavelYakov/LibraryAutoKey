using Library.Models;
using Microsoft.AspNetCore.Mvc;

namespace Library.Interfaces
{
    public interface IAuthorService
    {
        List<Author> GetAllAuthors();
        Author GetAuthorById(int id);
        List<Author> GetAuthorsByFilter(string? title, bool? isAvailable);
    }
}

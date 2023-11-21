using Library.Context;
using Library.Interfaces;
using Library.Models;
using Library.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
   
    public class AuthorController : ControllerBase
    {
        
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }
        

        [HttpGet("authors")]
        public IActionResult GetAllAuthors()
        {
            var Authors = _authorService.GetAllAuthors();
                 
                
            return Ok(Authors);
        }

        [HttpGet("author/{id}")]
        public IActionResult GetAuthorById(int id)
        {
            
            var author = _authorService.GetAuthorById(id);

            if (author == null)
            {
                return NotFound();
            }

            return Ok(author);

        }

        [HttpGet("filter-authors")]
        public IActionResult GetAuthorsByFilter(string? title, bool? isAvailable)
        {
            
            var filtresbooks = _authorService.GetAuthorsByFilter(title, isAvailable);
            return Ok(filtresbooks);
        }


    }
}

using Microsoft.AspNetCore.Mvc;
using OnlineBookStoreAPI.Data.Models;
using OnlineBookStoreAPI.Data;
using OnlineBookStoreAPI.Business;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]

/*
    We are going to assume that books can only be created by the admin of the site and that admin
    site is not part of this application. So users - logged in or otherwise can access a specific book
    or top books.
*/

public class BooksController : ControllerBase
{
    private readonly AppDbContext context;
    private readonly BookService bookService;

    public BooksController(AppDbContext context, BookService bookService)
    {
        this.context = context;
        this.bookService = bookService;
    }

    [HttpGet("TopBooks/{topNumber}")]
    public async Task<ActionResult<IEnumerable<Book>>> GetTopBooks(int topNumber)
    {
        var topBooks = await bookService.GetTopBooksAsync(topNumber);

        return Ok(topBooks);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Book>> GetBook(Guid id)
    {
        var foundBook = await context.Books.FirstOrDefaultAsync(b => b.Id == id);

        if (foundBook == null)
        {
            return NotFound();
        }

        return Ok(foundBook);
    }
}

using Microsoft.AspNetCore.Mvc;
using OnlineBookStoreAPI.Data.Models;
using OnlineBookStoreAPI.Data;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly AppDbContext _context;

    public BooksController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Book>> GetBooks()
    {
        return _context.Books.ToList();
    }

    [HttpGet("{id}")]
    public ActionResult<Book> GetBook(Guid id)
    {
        var book = _context.Books.FirstOrDefault(b => b.Id == id);

        if (book == null)
        {
            return NotFound();
        }

        return book;
    }
}

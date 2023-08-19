using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    // GET: api/books
    [HttpGet]
    public ActionResult<IEnumerable<Book>> GetBooks()
    {
        return _context.Books
            .Include(b => b.OrderDetails)
            .Include(b => b.Reviews)
            .ToList();
    }

    // GET: api/books/5
    [HttpGet("{id}")]
    public ActionResult<Book> GetBook(Guid id)
    {
        var book = _context.Books
            .Include(b => b.OrderDetails)
            .Include(b => b.Reviews)
            .FirstOrDefault(b => b.Id == id);

        if (book == null)
        {
            return NotFound();
        }

        return book;
    }
}

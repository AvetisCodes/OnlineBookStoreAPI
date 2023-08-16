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
    public ActionResult<Book> GetBook(int id)
    {
        var book = _context.Books.Find(id);

        if (book == null)
        {
            return NotFound();
        }

        return book;
    }

    // POST: api/books
    [HttpPost]
    public ActionResult<Book> CreateBook(Book book)
    {
        _context.Books.Add(book);
        _context.SaveChanges();

        return CreatedAtAction("GetBook", new { id = book.Id }, book);
    }

    // PUT: api/books/5
    [HttpPut("{id}")]
    public IActionResult UpdateBook(int id, Book book)
    {
        if (id != book.Id)
        {
            return BadRequest();
        }

        _context.Entry(book).State = EntityState.Modified;

        try
        {
            _context.SaveChanges();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BookExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    private bool BookExists(int id)
    {
        return _context.Books.Any(e => e.Id == id);
    }
}

using Microsoft.EntityFrameworkCore;
using OnlineBookStoreAPI.Data;
using OnlineBookStoreAPI.Data.Models;

namespace OnlineBookStoreAPI.Business
{
    public class BookService
    {
        private readonly AppDbContext _context;

        public BookService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Book>> GetTopBooksAsync(int topNumber)
        {
            if (topNumber < 0 || topNumber > 5)
            {
                throw new InvalidDataException("Top number has to be greater than 0 and less than 5.");
            }

            var topBooks = await _context.Books
                .Select(b => new
                {
                    Book = b,
                    AverageRating = b.Reviews.Average(r => r.Rating),
                    ReviewCount = b.Reviews.Count()
                })
                .Where(b => b.ReviewCount > 0)
                .OrderByDescending(b => b.AverageRating * b.ReviewCount)
                .Take(topNumber)
                .Select(b => b.Book)
                .ToListAsync();

            return topBooks;
        }
    }
}
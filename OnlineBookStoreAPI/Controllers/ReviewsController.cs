using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineBookStoreAPI.Data.Models;
using OnlineBookStoreAPI.Data;

namespace OnlineBookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReviewsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Reviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
        {
            return await _context.Reviews
                .Select(r => new Review
                {
                    Id = r.Id,
                    BookId = r.BookId,
                    UserId = r.UserId,
                    Rating = r.Rating,
                    Text = r.Text,
                    // You can add navigation properties as needed
                })
                .ToListAsync();
        }

        // GET: api/Reviews/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetReview(string id)
        {
            var review = await _context.Reviews.FindAsync(id);

            if (review == null)
            {
                return NotFound();
            }

            var reviewDto = new Review
            {
                Id = review.Id,
                BookId = review.BookId,
                UserId = review.UserId,
                Rating = review.Rating,
                Text = review.Text,
                // Navigation properties as needed
            };

            return reviewDto;
        }

        // POST: api/Reviews
        [HttpPost]
        public async Task<ActionResult<Review>> PostReview(Review reviewDto)
        {
            var review = new Review
            {
                BookId = reviewDto.BookId,
                UserId = reviewDto.UserId,
                Rating = reviewDto.Rating,
                Text = reviewDto.Text
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            reviewDto.Id = review.Id;
            return CreatedAtAction(nameof(GetReview), new { id = review.Id }, reviewDto);
        }

        // DELETE: api/Reviews/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

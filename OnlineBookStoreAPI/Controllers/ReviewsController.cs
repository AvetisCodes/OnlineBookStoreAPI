using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineBookStoreAPI.Data.Models;
using OnlineBookStoreAPI.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using OnlineBookStoreAPI.Data.DTOs;

namespace OnlineBookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly AppDbContext context;

        public ReviewsController(AppDbContext context)
        {
            this.context = context;
        }

        [Authorize]
        [HttpGet("MyReviews")]
        public async Task<ActionResult<IEnumerable<Review>>> GetUserReviews()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId is null)
            {
                return Unauthorized();
            }

            var reviews = await context.Reviews.Where(r => r.UserId == new Guid(userId)).ToListAsync();

            return Ok(reviews);
        }

        [HttpGet("Book/{bookId}")]
        public async Task<ActionResult<IEnumerable<Review>>> GetBookReviews(Guid bookId)
        {
            var currentBook = await context.Books.SingleOrDefaultAsync(b => b.Id == bookId);
            
            if (currentBook is null)
            {
                return NotFound();
            }

            var reviews = await context.Reviews.Where(r => r.BookId == bookId).ToListAsync();

            return Ok(reviews);
        }

        [Authorize]
        [HttpPost("MyReview")]
        public async Task<ActionResult<Review>> PostUserReview(ReviewDTO reviewDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId is null)
            {
                return Unauthorized();
            }

            var reviewModel = new Review
            {
                Id = Guid.NewGuid(),
                BookId = reviewDto.BookId,
                UserId = new Guid(userId),
                Rating = reviewDto.Rating,
                Text = reviewDto.Text
            };

            await context.SaveChangesAsync();

            var myNewReview = await context.Reviews.FirstOrDefaultAsync(r => r.Id == reviewModel.Id);

            return Ok(reviewDto);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserReview(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId is null)
            {
                return Unauthorized();
            }

            var review = await context.Reviews.FindAsync(id);

            if (review == null)
            {
                return NotFound();
            }

            if (review.UserId != new Guid(userId))
            {
                return Forbid();
            }

            context.Reviews.Remove(review);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}

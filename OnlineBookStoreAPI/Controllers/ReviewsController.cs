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

        [HttpGet("Book/{bookId}")]
        public async Task<ActionResult<IEnumerable<Review>>> GetBookReviews(Guid bookId)
        {
            var currentBook = await context.Books.SingleOrDefaultAsync(b => b.Id == bookId);

            if (currentBook is null)
            {
                return NotFound();
            }

            var reviews = await context.Reviews.Where(r => r.Book.Id == bookId).ToListAsync();

            return Ok(reviews);
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

            var reviews = await context.Reviews.Where(r => r.User.Id == new Guid(userId)).ToListAsync();

            return Ok(reviews);
        }

        [Authorize]
        [HttpPost("MyReview/{bookId}")]
        public async Task<ActionResult<Review>> PostUserReview(Guid bookId, [FromBody] ReviewDTO reviewDto)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId is null)
                {
                    return Unauthorized();
                }

                var existingBook = await context.Books.FindAsync(bookId);

                if (existingBook == null)
                {
                    return NotFound("Book not found.");
                }

                var currentUserModel = await context.Users.FindAsync(new Guid(userId));

                if (currentUserModel == null)
                {
                    return NotFound("User not found.");
                }

                // Just to keep things simple we will not be checking if user left review previously and will allow multiple reviews

                var reviewModel = new Review
                {
                    Id = Guid.NewGuid(),
                    Book = existingBook,
                    User = currentUserModel,
                    Rating = reviewDto.Rating,
                    Text = reviewDto.Text
                };

                context.Reviews.Add(reviewModel);
                await context.SaveChangesAsync();

                var foundReview = await context.Reviews.FirstOrDefaultAsync(b => b.Id == reviewModel.Id);

                return Ok(foundReview);
            }

            return BadRequest("Invalid data.");
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

            if (review.User.Id != new Guid(userId))
            {
                return Forbid();
            }

            context.Reviews.Remove(review);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineBookStoreAPI.Data.Models;
using OnlineBookStoreAPI.Data;
using Microsoft.AspNetCore.Authorization;
using OnlineBookStoreAPI.Data.DTOs;
using Microsoft.AspNetCore.Identity;

namespace OnlineBookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly UserManager<User> userManager;

        public ReviewsController(AppDbContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
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
            var currentUser = await userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return Unauthorized();
            }

            var reviews = await context.Reviews.Where(r => r.User.Id == currentUser.Id).ToListAsync();

            return Ok(reviews);
        }

        [Authorize]
        [HttpPost("MyReview/{bookId}")]
        public async Task<ActionResult<Review>> PostUserReview(Guid bookId, [FromBody] ReviewDTO reviewDto)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await userManager.GetUserAsync(User);

                if (currentUser == null)
                {
                    return Unauthorized();
                }

                var existingBook = await context.Books.FindAsync(bookId);

                if (existingBook == null)
                {
                    return NotFound("Book not found.");
                }

                // Just to keep things simple we will not be checking if user left review previously and will allow multiple reviews

                var reviewModel = new Review
                {
                    Id = Guid.NewGuid(),
                    Book = existingBook,
                    User = currentUser,
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
            var currentUser = await userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return Unauthorized();
            }

            var review = await context.Reviews.FindAsync(id);

            if (review == null)
            {
                return NotFound();
            }

            if (review.User.Id != currentUser.Id)
            {
                return Forbid();
            }

            context.Reviews.Remove(review);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}

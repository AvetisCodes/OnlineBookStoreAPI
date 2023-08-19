using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineBookStoreAPI.Data.Models;
using OnlineBookStoreAPI.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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

        // 3. Endpoint for authorized users to post or edit their review
        //[Authorize]
        //[HttpPost("MyReview")]
        //public async Task<ActionResult<Review>> PostOrEditUserReview(ReviewDTO reviewDto)
        //{
        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //    var existingReview = await _context.Reviews
        //        .FirstOrDefaultAsync(r => r.BookId == reviewDto.BookId && r.UserId == userId);

        //    if (existingReview == null)
        //    {
        //        var reviewModel = new Review
        //        {
        //            Id = Guid.NewGuid(),
        //            BookId = reviewDto.BookId,
        //            UserId = userId,
        //            Rating = reviewDto.Rating,
        //            Text = reviewDto.Text
        //        };

        //        _context.Reviews.Add(reviewModel);
        //    }
        //    else
        //    {
        //        existingReview.Rating = reviewDto.Rating;
        //        existingReview.Text = reviewDto.Text;
        //    }

        //    await _context.SaveChangesAsync();

        //    return Ok(reviewDto);
        //}

        // 4. Endpoint for authorized users to delete their review
        //[Authorize]
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteUserReview(Guid id)
        //{
        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //    var review = await _context.Reviews.FindAsync(id);

        //    if (review == null)
        //    {
        //        return NotFound();
        //    }

        //    if (review.UserId != userId)
        //    {
        //        return Forbid();
        //    }

        //    _context.Reviews.Remove(review);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}
    }
}

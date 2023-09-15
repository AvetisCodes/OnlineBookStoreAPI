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
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext context;

        public OrdersController(AppDbContext context)
        {
            this.context = context;
        }

        [Authorize]
        [HttpGet("MyOrders")]
        public async Task<ActionResult<IEnumerable<Order>>> GetUserOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId is null)
            {
                return Unauthorized();
            }

            var orders = await context.Orders.Include(o => o.OrderDetails).Where(r => r.User.Id == new Guid(userId)).ToListAsync();

            return Ok(orders);
        }

        [Authorize]
        [HttpPost("MyOrders")]
        public async Task<ActionResult<Order>> PostOrder(OrderDTO orderDto)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId is null)
                {
                    return Unauthorized();
                }

                var currentUserModel = await context.Users.FindAsync(new Guid(userId));

                if (currentUserModel == null)
                {
                    return NotFound("User not found.");
                }

                // Fetch actual books based on BookId
                var bookIds = orderDto.OrderDetailDTOs.Select(od => od.BookId).ToList();
                var books = await context.Books.Where(b => bookIds.Contains(b.Id)).ToDictionaryAsync(b => b.Id);

                // Calculate TotalPrice based on actual book prices
                decimal totalPrice = 0;
                var orderDetails = new List<OrderDetail>();

                foreach (var orderDetailDto in orderDto.OrderDetailDTOs)
                {
                    if (!books.ContainsKey(orderDetailDto.BookId))
                    {
                        return BadRequest($"Book with ID {orderDetailDto.BookId} not found.");
                    }

                    var book = books[orderDetailDto.BookId];
                    totalPrice += book.Price * orderDetailDto.Quantity;

                    orderDetails.Add(new OrderDetail
                    {
                        Book = book,
                        Quantity = orderDetailDto.Quantity
                    });
                }

                var orderModel = new Order
                {
                    Id = Guid.NewGuid(),
                    User = currentUserModel,
                    OrderDate = orderDto.OrderDate,
                    TotalPrice = totalPrice,
                    OrderDetails = orderDetails
                };

                context.Orders.Add(orderModel);
                await context.SaveChangesAsync();

                var foundOrder = await context.Orders.Include(o => o.OrderDetails).SingleOrDefaultAsync(r => r.Id == orderModel.Id);

                return Ok(foundOrder);
            }

            return BadRequest("Invalid data.");
        }
    }
}
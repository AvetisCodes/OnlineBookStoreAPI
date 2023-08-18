using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineBookStoreAPI.Data.Models;
using OnlineBookStoreAPI.Data;

namespace OnlineBookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)
                .Select(o => new Order
                {
                    Id = o.Id,
                    UserId = o.UserId,
                    OrderDate = o.OrderDate,
                    TotalPrice = o.TotalPrice,
                    OrderDetails = o.OrderDetails.Select(od => new OrderDetail
                    {
                        Id = od.Id,
                        BookId = od.BookId,
                        Quantity = od.Quantity
                    }).ToList()
                })
                .ToListAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(string id)
        {
            var order = await _context.Orders.Include(o => o.OrderDetails).FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            var orderDto = new Order
            {
                Id = order.Id,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,
                OrderDetails = order.OrderDetails.Select(od => new OrderDetail
                {
                    Id = od.Id,
                    BookId = od.BookId,
                    Quantity = od.Quantity
                }).ToList()
            };

            return orderDto;
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order orderDto)
        {
            var order = new Order
            {
                UserId = orderDto.UserId,
                OrderDate = orderDto.OrderDate,
                TotalPrice = orderDto.TotalPrice,
                OrderDetails = orderDto.OrderDetails.Select(od => new OrderDetail
                {
                    BookId = od.BookId,
                    Quantity = od.Quantity
                }).ToList()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            orderDto.Id = order.Id;
            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, orderDto);
        }
    }
}
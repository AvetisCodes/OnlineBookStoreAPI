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
        private readonly AppDbContext context;

        public OrdersController(AppDbContext context)
        {
            this.context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(Guid id)
        {
            var order = await context.Orders.Include(o => o.OrderDetails).FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        //[HttpPost]
        //public async Task<ActionResult<Order>> PostOrder(Order orderDto)
        //{
        //    var order = new Order
        //    {
        //        UserId = orderDto.UserId,
        //        OrderDate = orderDto.OrderDate,
        //        TotalPrice = orderDto.TotalPrice,
        //        OrderDetails = orderDto.OrderDetails.Select(od => new OrderDetail
        //        {
        //            BookId = od.BookId,
        //            Quantity = od.Quantity
        //        }).ToList()
        //    };

        //    context.Orders.Add(order);
        //    await context.SaveChangesAsync();

        //    orderDto.Id = order.Id;
        //    return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, orderDto);
        //}
    }
}
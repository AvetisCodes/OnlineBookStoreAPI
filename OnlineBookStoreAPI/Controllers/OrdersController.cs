﻿using Microsoft.AspNetCore.Mvc;
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
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly UserManager<User> userManager;

        public OrdersController(AppDbContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        [Authorize]
        [HttpGet("MyOrders")]
        public async Task<ActionResult<IEnumerable<Order>>> GetUserOrders()
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            var orders = await context.Orders.Where(r => r.User.Id == user.Id).ToListAsync();

            return Ok(orders);
        }

        [Authorize]
        [HttpPost("MyOrders")]
        public async Task<ActionResult<Order>> PostOrder([FromBody] OrderDTO orderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data.");
            }

            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            // Fetch actual books based on BookId
            var bookIds = orderDto.OrderDetailDTOs.Select(od => od.BookId).ToList();
            var booksDictionary = await context.Books.Where(b => bookIds.Contains(b.Id)).ToDictionaryAsync(b => b.Id);

            var order = new Order
            {
                Id = Guid.NewGuid(),
                OrderDate = orderDto.OrderDate,
                User = user,
            };

            foreach (var orderDetailDto in orderDto.OrderDetailDTOs)
            {
                if (!booksDictionary.ContainsKey(orderDetailDto.BookId))
                {
                    return BadRequest($"No book found with ID {orderDetailDto.BookId}");
                }

                var book = booksDictionary[orderDetailDto.BookId];

                var orderDetail = new OrderDetail
                {
                    Id = Guid.NewGuid(),
                    Book = book,
                    Quantity = orderDetailDto.Quantity,
                };

                order.OrderDetails.Add(orderDetail);
                order.TotalPrice += book.Price * orderDetailDto.Quantity;
            }

            context.Orders.Add(order);
            await context.SaveChangesAsync();

            var savedOrder = await context.Orders.FirstOrDefaultAsync(o => o.Id == order.Id);

            return Ok(savedOrder);
        }
    }
}
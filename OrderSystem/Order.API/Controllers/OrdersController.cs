using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Order.API.Data;
using Order.API.Models;
using Order.API.Services;
using Shared.Contracts;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderDbContext _context;

        public OrdersController(OrderDbContext dbContext)
        {
            _context = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderEntity order, [FromServices] KafkaProducer producer)
        {
            order.Status = "Created";

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var eventMessage = new OrderCreatedEvent
            {
                OrderId = order.Id,
                ProductName = order.ProductName,
                Quantity = order.Quantity
            };

            await producer.ProducerAsync("orders", eventMessage);

            return Ok(order);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
                return NotFound();

            return Ok(order);
        }
    }
}

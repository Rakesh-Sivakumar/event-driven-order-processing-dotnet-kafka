using Microsoft.EntityFrameworkCore;
using Order.Application.Interfaces;
using Order.Domain.Entities;

namespace Order.Infrastructure.Data
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _context;

        public OrderRepository(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetStockAsync(string productName)
        {
            return await _context.Orders
                .Where(x => x.ProductName == productName)
                .Select(x => x.Quantity)
                .FirstOrDefaultAsync();
        }

        public async Task CreateOrderAsync(OrderEntity order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }
    }
}
using Order.Domain.Entities;

namespace Order.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<int> GetStockAsync(string productName);
        Task CreateOrderAsync(OrderEntity order);
    }
}

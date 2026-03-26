using Order.Domain.Entities;

namespace Order.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderEntity> CreateOrderAsync(OrderEntity order);
    }
}

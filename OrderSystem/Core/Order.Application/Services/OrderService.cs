using Microsoft.Extensions.Logging;
using Order.Application.Interfaces;
using Order.Domain.Entities;
using Shared.Contracts;

namespace Order.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly IRedisService _redisService;
        private readonly IMessageProducer _messageProducer;
        private readonly ILogger<OrderService> _logger;

        public OrderService(
            IOrderRepository orderRepository,
            IRedisService redisService,
            IMessageProducer messageProducer,
            ILogger<OrderService> logger)
        {
            _repository = orderRepository;
            _redisService = redisService;
            _messageProducer = messageProducer;
            _logger = logger;
        }

        public async Task<OrderEntity> CreateOrderAsync(OrderEntity order)
        {
            var cacheKey = $"inventory:{order.ProductName}";
            int stock;

            //Check Redis
            var cachedStock = await _redisService.GetAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedStock))
            {
                _logger.LogInformation("Cache hit (OrderService)");
                stock = int.Parse(cachedStock);
            }
            else
            {
                _logger.LogInformation("Cache miss → DB hit");

                stock = await _repository.GetStockAsync(order.ProductName!);

                if (stock == 0)
                    throw new Exception("Product not found");

                // Save to Redis
                await _redisService.SetAsync(cacheKey, stock.ToString());
            }

            //Validate stock
            if (stock < order.Quantity)
                throw new Exception("Out of stock");

            //Create Order
            order.Status = "Created";

            await _repository.CreateOrderAsync(order);

            _logger.LogInformation($"Order created with ID: {order.Id}");

            //Publish Event to Kafka
            var eventMessage = new OrderCreatedEvent
            {
                OrderId = order.Id,
                ProductName = order.ProductName,
                Quantity = order.Quantity
            };

            await _messageProducer.ProducerAsync("orders", eventMessage);

            _logger.LogInformation("OrderCreatedEvent published to Kafka");

            return order;
        }
    }

}

using Entities;
using Microsoft.Extensions.Logging;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class OrderService : IOrderService
    {
        private IOrderRepository _orderRepository;
        private IProductService _productService;
        private readonly ILogger<OrderService> _logger;
        public OrderService(IOrderRepository orderRepository, ILogger<OrderService> logger, IProductService productService)
        {
            _orderRepository = orderRepository;
            _productService = productService;
            _logger = logger;
        }
        public async Task<Order> CreateOrder(Order order)
        {
            double order_sum = 0;

            foreach (OrderItem i in order.OrderItems)
            {
                Product product = await _productService.GetProductById(i.ProductId);
                order_sum += product.Price * i.Quantity;
            }
            if (order_sum != order.OrderSum)
            {
                _logger.LogError($"user {order.UserId}  tried perchasing with a difffrent price {order.OrderSum} instead of {order_sum}");
                _logger.LogInformation($"user {order.UserId}  tried perchasing with a difffrent price {order.OrderSum} instead of {order_sum}");
            }
            order.OrderSum = order_sum;
            return await _orderRepository.CreateOrder(order);
        }
    }
}

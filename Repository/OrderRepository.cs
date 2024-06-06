using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class OrderRepository : IOrderRepository
    {
        private AdoNetUsers214956807Context _ordersContext;
        public OrderRepository(AdoNetUsers214956807Context ordersContext)
        {
            _ordersContext = ordersContext;
        }
        public async Task<Order> CreateOrder(Order order)
        {
            await _ordersContext.Orders.AddAsync(order);
            await _ordersContext.SaveChangesAsync();
            return order;
        }
    }
}

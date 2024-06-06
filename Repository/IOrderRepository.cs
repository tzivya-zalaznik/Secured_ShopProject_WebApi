using Entities;

namespace Repository
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrder(Order order);
    }
}
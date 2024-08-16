using Orders.API.Models;

namespace Orders.API.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync(int profileId);
        Task<Order> GetOrderByIdAsync(int id);
        Task<int> AddOrderAsync(Order Order);
        Task<bool> UpdateOrderAsync(Order Order);
        Task<bool> DeleteOrderAsync(int id);
        Task<int> DeleteOrdersByProfileAsync(int profileid);
    }
}

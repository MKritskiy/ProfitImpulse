using Orders.API.Models;

namespace Orders.API.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetOrdersAsync(int profileid, string jwtToken);
    }
}

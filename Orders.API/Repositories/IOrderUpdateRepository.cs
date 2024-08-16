using Orders.API.Models;

namespace Orders.API.Repositories
{
    public interface IOrderUpdateRepository
    {
        Task<OrderUpdate> GetLatestUpdateAsync(int profileId);
        Task<int> AddOrderUpdateAsync(OrderUpdate OrderUpdate);
        Task<bool> DeleteOrderUpdatesByProfileAsync(int profileId);
    }
}

using Inventories.API.Models;

namespace Inventories.API.Repositories
{
    public interface IStockUpdateRepository
    {
        Task<StockUpdate> GetLatestUpdateAsync(int profileId);
        Task<int> AddStockUpdateAsync(StockUpdate stockUpdate);
        Task<bool> DeleteStockUpdatesByProfileAsync(int profileId);
    }
}

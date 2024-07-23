using Inventories.API.Models;

namespace Inventories.API.Repositories
{
    public interface IStockRepository
    {
        Task<IEnumerable<Stock>> GetAllStocksAsync(int profileId);
        Task<Stock> GetStockByIdAsync(int id);
        Task<int> AddStockAsync(Stock stock);
        Task<bool> UpdateStockAsync(Stock stock);
        Task<bool> DeleteStockAsync(int id);
        Task<int> DeleteStocksByProfileAsync(int profileid);
    }
}

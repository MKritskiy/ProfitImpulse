using Inventories.API.Models;

namespace Inventories.API.Services
{
    public interface IStockService
    {
        Task<IEnumerable<Stock>> GetStocksAsync(int profileid);
    }
}

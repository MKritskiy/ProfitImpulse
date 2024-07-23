using Inventories.API.Database;
using Inventories.API.Models;

namespace Inventories.API.Repositories
{
    public class StockRepository : IStockRepository
    {
        public async Task<IEnumerable<Stock>> GetAllStocksAsync(int profileId)
        {
            const string query = "SELECT * FROM Stocks WHERE profile_id = @ProfileId";
            return await DbHelper.QueryAsync<Stock>(query, new { ProfileId = profileId });
        }

        public async Task<Stock> GetStockByIdAsync(int id)
        {
            const string query = "SELECT * FROM Stocks WHERE stock_id = @Id";
            return await DbHelper.QueryFirstOrDefaultAsync<Stock>(query, new { Id = id }) ?? new Stock();
        }

        public async Task<int> AddStockAsync(Stock stock)
        {
            const string query = @"
            INSERT INTO Stocks (profile_id, warehouse_name, product_quantity, product_name, product_sku, last_update, lifetime_minutes) 
            VALUES (@ProfileId, @WarehouseName, @ProductQuantity, @ProductName, @ProductSku, @LastUpdate, @LifetimeMinutes)
            RETURNING stock_id";
            return await DbHelper.QueryFirstOrDefaultAsync<int>(query, stock);
        }

        public async Task<bool> UpdateStockAsync(Stock stock)
        {
            const string query = @"
            UPDATE Stocks 
            SET profile_id = @ProfileId, warehouse_name = @WarehouseName, product_quantity = @ProductQuantity, 
                product_name = @ProductName, product_sku = @ProductSku, last_update = @LastUpdate, lifetime_minutes = @LifetimeMinutes 
            WHERE stock_id = @StockId";
            int rowsAffected = await DbHelper.ExecuteAsync(query, stock);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteStockAsync(int id)
        {
            const string query = "DELETE FROM Stocks WHERE stock_id = @Id";
            int rowsAffected = await DbHelper.ExecuteAsync(query, new { Id = id });
            return rowsAffected > 0;
        }

        public async Task<int> DeleteStocksByProfileAsync(int profileid)
        {
            const string query = "DELETE FROM Stocks WHERE profile_id = @Id";
            int rowsAffected = await DbHelper.ExecuteAsync(query, new { ProfileId = profileid });
            return rowsAffected;
        }
    }

    
}

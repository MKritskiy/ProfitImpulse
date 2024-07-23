using Inventories.API.Database;
using Inventories.API.Models;

namespace Inventories.API.Repositories
{
    public class StockUpdateRepository : IStockUpdateRepository
    {

        public async Task<StockUpdate> GetLatestUpdateAsync(int profileId)
        {
            const string query = "SELECT * FROM StockUpdates WHERE profile_id = @ProfileId ORDER BY last_update DESC LIMIT 1";
            return await DbHelper.QueryFirstOrDefaultAsync<StockUpdate>(query, new { ProfileId = profileId }) ?? new StockUpdate();
        }

        public async Task<int> AddStockUpdateAsync(StockUpdate stockUpdate)
        {
            const string query = @"
            INSERT INTO StockUpdates (profile_id, last_update, lifetime_minutes, date_from) 
            VALUES (@ProfileId, @LastUpdate, @LifetimeMinutes, @DateFrom)
            RETURNING update_id";
            return await DbHelper.QueryFirstOrDefaultAsync<int>(query, stockUpdate);
        }

        public async Task<bool> DeleteStockUpdatesByProfileAsync(int profileId)
        {
            const string query = "DELETE FROM StockUpdates WHERE profile_id = @ProfileId";
            int rowsAffected = await DbHelper.ExecuteAsync(query, new { ProfileId = profileId });
            return rowsAffected > 0;
        }
    }

}

using Helpers.Database;
using Orders.API.Models;

namespace Orders.API.Repositories
{
    public class OrderUpdateRepository : IOrderUpdateRepository
    {

        public async Task<OrderUpdate> GetLatestUpdateAsync(int profileId)
        {
            const string query = "SELECT * FROM OrderUpdates WHERE profile_id = @ProfileId ORDER BY last_update DESC LIMIT 1";
            return await DbHelper.QueryFirstOrDefaultAsync<OrderUpdate>(query, new { ProfileId = profileId }) ?? new OrderUpdate();
        }

        public async Task<int> AddOrderUpdateAsync(OrderUpdate OrderUpdate)
        {
            const string query = @"
            INSERT INTO OrderUpdates (profile_id, last_update, lifetime_minutes, date_from) 
            VALUES (@ProfileId, @LastUpdate, @LifetimeMinutes, @DateFrom)
            RETURNING update_id";
            return await DbHelper.QueryFirstOrDefaultAsync<int>(query, OrderUpdate);
        }

        public async Task<bool> DeleteOrderUpdatesByProfileAsync(int profileId)
        {
            const string query = "DELETE FROM OrderUpdates WHERE profile_id = @ProfileId";
            int rowsAffected = await DbHelper.ExecuteAsync(query, new { ProfileId = profileId });
            return rowsAffected > 0;
        }
    }

}

using Purchases.API.Database;
using Purchases.API.Models;

namespace Purchases.API.Repositories
{
    public class PurchaseUpdateRepository : IPurchaseUpdateRepository
    {

        public async Task<PurchaseUpdate> GetLatestUpdateAsync(int profileId)
        {
            const string query = "SELECT * FROM PurchaseUpdates WHERE profile_id = @ProfileId ORDER BY last_update DESC LIMIT 1";
            return await DbHelper.QueryFirstOrDefaultAsync<PurchaseUpdate>(query, new { ProfileId = profileId }) ?? new PurchaseUpdate();
        }

        public async Task<int> AddPurchaseUpdateAsync(PurchaseUpdate purchaseUpdate)
        {
            const string query = @"
            INSERT INTO PurchaseUpdates (profile_id, last_update, lifetime_minutes, date_from) 
            VALUES (@ProfileId, @LastUpdate, @LifetimeMinutes, @DateFrom)
            RETURNING update_id";
            return await DbHelper.QueryFirstOrDefaultAsync<int>(query, purchaseUpdate);
        }

        public async Task<bool> DeletePurchaseUpdatesByProfileAsync(int profileId)
        {
            const string query = "DELETE FROM PurchaseUpdates WHERE profile_id = @ProfileId";
            int rowsAffected = await DbHelper.ExecuteAsync(query, new { ProfileId = profileId });
            return rowsAffected > 0;
        }
    }

}

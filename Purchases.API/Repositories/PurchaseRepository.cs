using Helpers.Database;
using Purchases.API.Models;

namespace Purchases.API.Repositories
{
    public class PurchaseRepository : IPurchaseRepository
    {
        public async Task<IEnumerable<Purchase>> GetAllPurchasesAsync(int profileId)
        {
            const string query = "SELECT * FROM Purchases WHERE profile_id = @ProfileId";
            return await DbHelper.QueryAsync<Purchase>(query, new { ProfileId = profileId });
        }

        public async Task<Purchase> GetPurchaseByIdAsync(int id)
        {
            const string query = "SELECT * FROM Purchases WHERE purchase_id = @Id";
            return await DbHelper.QueryFirstOrDefaultAsync<Purchase>(query, new { Id = id }) ?? new Purchase();
        }

        public async Task<int> AddPurchaseAsync(Purchase purchase)
        {
            const string query = @"
            INSERT INTO Purchases (profile_id, purchase_date, purchase_amount, purchase_name, 
            purchase_sku, purchase_brand, purchase_category, purchase_country, purchase_state, purchase_region) 
            VALUES (@ProfileId, @PurchaseDate, @PurchaseAmount, @PurchaseName, @PurchaseSku, 
            @PurchaseBrand, @PurchaseCategory, @PurchaseCountry, @PurchaseState, @PurchaseRegion)
            RETURNING purchase_id";
            return await DbHelper.QueryFirstOrDefaultAsync<int>(query, purchase);
        }

        public async Task<bool> UpdatePurchaseAsync(Purchase purchase)
        {
            const string query = @"
            UPDATE Purchases 
            SET profile_id = @ProfileId, purchase_date = @PurchaseDate,
                purchase_amount = @PurchaseAmount, purchase_name = @PurchaseName, purchase_sku = @PurchaseSku, 
                purchase_brand = @PurchaseBrand, purchase_category = @PurchaseCategory, purchase_country = @PurchaseCountry, 
                purchase_state = @PurchaseState, purchase_region = @PurchaseRegion
            WHERE purchase_id = @PurchaseId";
            int rowsAffected = await DbHelper.ExecuteAsync(query, purchase);
            return rowsAffected > 0;
        }

        public async Task<bool> DeletePurchaseAsync(int id)
        {
            const string query = "DELETE FROM Purchases WHERE purchase_id = @PurchaseId";
            int rowsAffected = await DbHelper.ExecuteAsync(query, new { PurchaseId = id });
            return rowsAffected > 0;
        }

        public async Task<int> DeletePurchasesByProfileAsync(int profileId)
        {
            const string query = "DELETE FROM Purchases WHERE profile_id = @ProfileId";
            int rowsAffected = await DbHelper.ExecuteAsync(query, new { ProfileId = profileId });
            return rowsAffected;
        }
    }


}

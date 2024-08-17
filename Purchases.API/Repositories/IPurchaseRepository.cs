using Purchases.API.Models;

namespace Purchases.API.Repositories
{
    public interface IPurchaseRepository
    {
        Task<IEnumerable<Purchase>> GetAllPurchasesAsync(int profileId);
        Task<Purchase> GetPurchaseByIdAsync(int id);
        Task<int> AddPurchaseAsync(Purchase purchase);
        Task<bool> UpdatePurchaseAsync(Purchase purchase);
        Task<bool> DeletePurchaseAsync(int id);
        Task<int> DeletePurchasesByProfileAsync(int profileid);
    }
}

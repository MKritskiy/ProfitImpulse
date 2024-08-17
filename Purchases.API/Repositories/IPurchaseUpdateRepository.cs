using Purchases.API.Models;

namespace Purchases.API.Repositories
{
    public interface IPurchaseUpdateRepository
    {
        Task<PurchaseUpdate> GetLatestUpdateAsync(int profileId);
        Task<int> AddPurchaseUpdateAsync(PurchaseUpdate purhcaseUpdate);
        Task<bool> DeletePurchaseUpdatesByProfileAsync(int profileId);
    }
}

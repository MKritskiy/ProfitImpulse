using Purchases.API.Models;

namespace Purchases.API.Services
{
    public interface IPurchaseService
    {
        Task<IEnumerable<Purchase>> GetPurchasesAsync(int profileid, string jwtToken);
    }
}

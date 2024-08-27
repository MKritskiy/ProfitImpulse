using Purchases.API.Models;
using Purchases.API.Repositories;
using Purchases.API.Dto;
using Helpers;

namespace Purchases.API.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IPurchaseUpdateRepository _purchaseUpdateRepository;
        private readonly IRequestApiHelper _requestApiHelper;

        public PurchaseService(IPurchaseRepository purchaseRepository, IPurchaseUpdateRepository purchaseUpdateRepository, IRequestApiHelper requestApiHelper)
        {
            _purchaseRepository = purchaseRepository;
            _purchaseUpdateRepository = purchaseUpdateRepository;
            _requestApiHelper = requestApiHelper;
        }

        public async Task<IEnumerable<Purchase>> GetPurchasesAsync(int profileid, string jwtToken)
        {
            try
            {
                var purchases = await _purchaseRepository.GetAllPurchasesAsync(profileid);
                var latestUpdate = await _purchaseUpdateRepository.GetLatestUpdateAsync(profileid);

                if (purchases == null || !purchases.Any() || CheckDataHelper.IsDataExpired(latestUpdate))
                {
                    var purchaseApi = await _requestApiHelper.FetchListFromApi<ApiPurchase>(PurchaseConstants.PurchasesQuery, profileid, jwtToken);

                    purchases = PurchaseDtoMapper.MapDtoListToModelList(purchaseApi);

                    await SavePurchases(purchases, profileid);
                    purchases = await _purchaseRepository.GetAllPurchasesAsync(profileid);
                }

                return purchases;
            }
            catch
            {
                throw;
            }

        }


        private async Task SavePurchases(IEnumerable<Purchase> Purchases, int profileid)
        {
            // Delete old Purchases
            await _purchaseRepository.DeletePurchasesByProfileAsync(profileid);
            await _purchaseUpdateRepository.DeletePurchaseUpdatesByProfileAsync(profileid);
            // Add new Purchases
            foreach (var Purchase in Purchases)
            {
                Purchase.ProfileId = profileid;
                Purchase.PurchaseId = await _purchaseRepository.AddPurchaseAsync(Purchase);
            }

            // Create PurchaseUpdate
            var PurchaseUpdate = new PurchaseUpdate
            {
                ProfileId = profileid,
                LastUpdate = DateTime.UtcNow,
                LifetimeMinutes = 30,
                DateFrom = new DateTime(2019, 6, 20)
            };

            await _purchaseUpdateRepository.AddPurchaseUpdateAsync(PurchaseUpdate);
        }
    }
}

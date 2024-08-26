using Purchases.API.Models;

namespace Purchases.API.Dto
{
    public class PurchaseDtoMapper
    {
        public static IEnumerable<Purchase> MapDtoListToModelList(IEnumerable<ApiPurchase> dtoList)
        {
            return dtoList
                .GroupBy(apiPurchase => new
                {
                    apiPurchase.Date,
                    apiPurchase.Subject,
                    apiPurchase.Barcode,
                    apiPurchase.Brand,
                    apiPurchase.Category,
                    apiPurchase.TotalPrice,
                    apiPurchase.CountryName,
                    apiPurchase.OblastOkrugName,
                    apiPurchase.RegionName,
                    apiPurchase.FinishedPrice
                })
                .Select(group => new Purchase
                {
                    PurchaseDate = group.Key.Date,
                    PurchaseName = group.Key.Subject ?? string.Empty,
                    PurchaseSku = group.Key.Barcode ?? string.Empty,
                    PurchaseBrand = group.Key.Brand ?? string.Empty,
                    PurchaseCategory = group.Key.Category ?? string.Empty,
                    PurchaseAmount = group.Key.TotalPrice * group.Count(),
                    PurchaseCountry = group.Key.CountryName ?? string.Empty,
                    PurchaseState = group.Key.OblastOkrugName ?? string.Empty,
                    PurchaseRegion = group.Key.RegionName ?? string.Empty,
                    PurchaseQuantity = group.Count()
                });
        }
    }
}

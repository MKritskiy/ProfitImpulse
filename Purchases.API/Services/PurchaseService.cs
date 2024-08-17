using Purchases.API.Models;
using Purchases.API.Repositories;
using Microsoft.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;
using Purchases.API.Dto;

namespace Purchases.API.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IPurchaseUpdateRepository _purchaseUpdateRepository;
        private readonly IHttpClientFactory _httpClientFactory;

        public PurchaseService(IPurchaseRepository purchaseRepository, IPurchaseUpdateRepository purchaseUpdateRepository, IHttpClientFactory httpClientFactory)
        {
            _purchaseRepository = purchaseRepository;
            _purchaseUpdateRepository = purchaseUpdateRepository;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<Purchase>> GetPurchasesAsync(int profileid, string jwtToken)
        {
            var Purchases = await _purchaseRepository.GetAllPurchasesAsync(profileid);
            var latestUpdate = await _purchaseUpdateRepository.GetLatestUpdateAsync(profileid);

            if (Purchases == null || !Purchases.Any() || isDataExpired(latestUpdate))
            {
                Purchases = await FetchPurchasesFromApi(profileid, jwtToken);
                await SavePurchases(Purchases, profileid);
                Purchases = await _purchaseRepository.GetAllPurchasesAsync(profileid);
            }

            return Purchases;
        }

        private bool isDataExpired(PurchaseUpdate latestUpdate)
        {
            if (latestUpdate.UpdateId < 0)
                return true;
            return DateTime.UtcNow > latestUpdate.LastUpdate.AddMinutes(latestUpdate.LifetimeMinutes);
        }

        private async Task<IEnumerable<Purchase>> FetchPurchasesFromApi(int profileid, string jwtToken)
        {
            var client = _httpClientFactory.CreateClient();
            // Get auth key
            string? authkey = await GetAuthorizationKey(profileid, jwtToken) ?? null;

            if (authkey == null)
                throw new Exception("Authorization key not found.");

            // Add auth key in header params
            client.DefaultRequestHeaders.Add(HeaderNames.Authorization, authkey);

            // Get Purchases from WB api
            var responce = await client.GetAsync(PurchaseConstants.PurchasesQuery);
            responce.EnsureSuccessStatusCode();
            var content = await responce.Content.ReadAsStringAsync();
            var apiPurchases = JsonSerializer.Deserialize<IEnumerable<ApiPurchase>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (apiPurchases == null)
                throw new Exception("Failed to fetch Purchases.");

            // Map dto to model with counting of repeated elements
            var purchases = apiPurchases
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
                    ProfileId = profileid,
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
                })
                .ToList();

            //var purchases = apiPurchases.Select(apiPurchase => new Purchase
            //{
            //    ProfileId = profileid,
            //    PurchaseDate = apiPurchase.Date,
            //    PurchaseName = apiPurchase.Subject ?? string.Empty,
            //    PurchaseSku = apiPurchase.Barcode ?? string.Empty,
            //    PurchaseBrand = apiPurchase.Brand ?? string.Empty,
            //    PurchaseCategory = apiPurchase.Category ?? string.Empty,
            //    PurchaseAmount = apiPurchase.TotalPrice, 
            //    PurchaseCountry = apiPurchase.CountryName ?? string.Empty,  
            //    PurchaseState = apiPurchase.OblastOkrugName ?? string.Empty,
            //    PurchaseRegion = apiPurchase.RegionName ?? string.Empty
            //}).ToList();
            
            return purchases;
        }

        private async Task<string?> GetAuthorizationKey(int profileid, string jwtToken)
        {
            var client = _httpClientFactory.CreateClient();
            var url = PurchaseConstants.ProfileAuthKeyQuery + profileid;
            try
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<JsonObject>();
                    return result?["apiKey"]?.ToString();
                }
                else
                {
                    Console.WriteLine($"Error: Received {response.StatusCode} for URL {url}");
                    return null;
                }
            }
            catch (HttpRequestException e)
            {
                // Log the exception message and the URL
                Console.WriteLine($"HttpRequestException: {e.Message} for URL {url}");
                return null;
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

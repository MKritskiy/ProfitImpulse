using Inventories.API.Dto;
using Inventories.API.Models;
using Inventories.API.Repositories;
using Microsoft.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Inventories.API.Services
{
    public class StockService : IStockService
    {
        private readonly IStockRepository _stockRepository;
        private readonly IStockUpdateRepository _stockUpdateRepository;
        private readonly IHttpClientFactory _httpClientFactory;

        public StockService(IStockRepository stockRepository, IStockUpdateRepository stockUpdateRepository, IHttpClientFactory httpClientFactory)
        {
            _stockRepository = stockRepository;
            _stockUpdateRepository = stockUpdateRepository;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<Stock>> GetStocksAsync(int profileid, string jwtToken)
        {
            var stocks = await _stockRepository.GetAllStocksAsync(profileid);
            var latestUpdate = await _stockUpdateRepository.GetLatestUpdateAsync(profileid);

            if (stocks == null || !stocks.Any() || isDataExpired(latestUpdate))
            {
                stocks = await FetchStocksFromApi(profileid, jwtToken);
                await SaveStocks(stocks, profileid);
                stocks = await _stockRepository.GetAllStocksAsync(profileid);
            }

            return stocks;
        }

        private bool isDataExpired(StockUpdate latestUpdate)
        {
            if (latestUpdate.UpdateId < 0)
                return true;
            return DateTime.UtcNow > latestUpdate.LastUpdate.AddMinutes(latestUpdate.LifetimeMinutes);
        }

        private async Task<IEnumerable<Stock>> FetchStocksFromApi(int profileid, string jwtToken)
        {
            var client = _httpClientFactory.CreateClient();
            // Get auth key
            string? authkey = await GetAuthorizationKey(profileid, jwtToken) ?? null;

            if (authkey == null)
                throw new Exception("Authorization key not found.");

            // Add auth key in header params
            client.DefaultRequestHeaders.Add(HeaderNames.Authorization, authkey);

            // Get stocks from WB api
            var responce = await client.GetAsync(StockConstants.StocksQuery);
            responce.EnsureSuccessStatusCode();
            var content = await responce.Content.ReadAsStringAsync();
            var apiStocks = JsonSerializer.Deserialize<IEnumerable<ApiStock>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (apiStocks == null)
                throw new Exception("Failed to fetch stocks.");

            // Map dto to model
            var stocks = apiStocks.Select(apiStock => new Stock
            {
                ProfileId = profileid,
                WarehouseName = apiStock.WarehouseName,
                ProductQuantity = apiStock.Quantity,
                ProductName = apiStock.Category,
                ProductSku = apiStock.Barcode,
                LastUpdate = DateTime.UtcNow,
                LifetimeMinutes = 30
            }).ToList();

            return stocks;
        }

        private async Task<string?> GetAuthorizationKey(int profileid, string jwtToken)
        {
            var client = _httpClientFactory.CreateClient();
            var url = StockConstants.ProfileAuthKeyQuery + profileid;
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

        private async Task SaveStocks(IEnumerable<Stock> stocks, int profileid)
        {
            // Delete old stocks
            await _stockRepository.DeleteStocksByProfileAsync(profileid);
            await _stockUpdateRepository.DeleteStockUpdatesByProfileAsync(profileid);
            // Add new stocks
            foreach (var stock in stocks)
            {
                stock.StockId = await _stockRepository.AddStockAsync(stock);
            }

            // Create StockUpdate
            var stockUpdate = new StockUpdate
            {
                ProfileId = profileid,
                LastUpdate = DateTime.UtcNow,
                LifetimeMinutes = 30,
                DateFrom = new DateTime(2019, 6, 20)
            };

            await _stockUpdateRepository.AddStockUpdateAsync(stockUpdate);
        }
    }
}

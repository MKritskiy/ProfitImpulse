using Inventories.API.Dto;
using Inventories.API.Models;
using Inventories.API.Repositories;
using Microsoft.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;
using static System.Net.Mime.MediaTypeNames;

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

        public async Task<IEnumerable<Stock>> GetStocksAsync(int profileid)
        {
            var stocks = await _stockRepository.GetAllStocksAsync(profileid);
            var latestUpdate = await _stockUpdateRepository.GetLatestUpdateAsync(profileid);
            
            if (stocks == null || !stocks.Any() || isDataExpired(latestUpdate)) 
            {
                stocks = await FetchStocksFromApi(profileid);
                await SaveStocks(stocks, profileid);
            }

            return stocks;
        }

        private bool isDataExpired(StockUpdate latestUpdate)
        {
            if (latestUpdate.UpdateId < 0)
                return true;
            return DateTime.UtcNow > latestUpdate.LastUpdate.AddMinutes(latestUpdate.LifetimeMinutes);
        }

        private async Task<IEnumerable<Stock>> FetchStocksFromApi(int profileid)
        {
            var client = _httpClientFactory.CreateClient();
            // Get auth key
            string? authkey = await GetAuthorizationKey(profileid) ?? null;

            if (authkey == null)
                throw new Exception();

            // Add auth key in header params
            client.DefaultRequestHeaders.Add(HeaderNames.Authorization, authkey);

            // Get stocks from WB api
            var responce = await client.GetAsync(StockConstants.GetStocksQuery);
            responce.EnsureSuccessStatusCode();
            var content = await responce.Content.ReadAsStringAsync();
            var apiStocks = JsonSerializer.Deserialize<IEnumerable<ApiStock>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
            if (apiStocks == null)
                throw new Exception();

            // Map dto to model
            var stocks = apiStocks.Select(apiStock => new Stock
            {
                ProfileId = profileid,
                WarehouseName = apiStock.WarehouseName,
                ProductQuantity = apiStock.Quantity,
                ProductName = apiStock.Subject,
                ProductSku = apiStock.Barcode,
                LastUpdate = DateTime.UtcNow,
                LifetimeMinutes = 30
            }).ToList();

            return stocks;
        }

        private async Task<string?> GetAuthorizationKey(int profileid)
        {
            var client = _httpClientFactory.CreateClient();
            var result = await client.GetFromJsonAsync<JsonObject>($"/profile/{profileid}"); 
            return result?["authkey"]?.ToString();
        }

        private async Task SaveStocks(IEnumerable<Stock> stocks, int profileid)
        {
            // Delete old stocks
            await _stockRepository.DeleteStocksByProfileAsync(profileid);
            await _stockUpdateRepository.DeleteStockUpdatesByProfileAsync(profileid);
            // Add new stocks
            foreach ( var stock in stocks)
            {
                await _stockRepository.AddStockAsync(stock);
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

using Helpers;
using Inventories.API.Dto;
using Inventories.API.Models;
using Inventories.API.Repositories;

namespace Inventories.API.Services
{
    public class StockService : IStockService
    {
        private readonly IStockRepository _stockRepository;
        private readonly IStockUpdateRepository _stockUpdateRepository;
        private readonly IRequestApiHelper _requestApiHelper;

        public StockService(IStockRepository stockRepository, IStockUpdateRepository stockUpdateRepository, IRequestApiHelper requestApiHelper)
        {
            _stockRepository = stockRepository;
            _stockUpdateRepository = stockUpdateRepository;
            _requestApiHelper = requestApiHelper;
        }

        public async Task<IEnumerable<Stock>> GetStocksAsync(int profileid, string jwtToken)
        {
            var stocks = await _stockRepository.GetAllStocksAsync(profileid);
            var latestUpdate = await _stockUpdateRepository.GetLatestUpdateAsync(profileid);

            if (stocks == null || !stocks.Any() || CheckDataHelper.IsDataExpired(latestUpdate))
            {
                var stocksApi = await _requestApiHelper.FetchListFromApi<ApiStock>(StockConstants.StocksQuery, profileid, jwtToken);

                stocks = StockDtoMapper.MapDtoListToModelList(stocksApi);

                await SaveStocks(stocks, profileid);
                stocks = await _stockRepository.GetAllStocksAsync(profileid);
            }

            return stocks;
        }

        private async Task SaveStocks(IEnumerable<Stock> stocks, int profileid)
        {
            // Delete old stocks
            await _stockRepository.DeleteStocksByProfileAsync(profileid);
            await _stockUpdateRepository.DeleteStockUpdatesByProfileAsync(profileid);
            // Add new stocks
            foreach (var stock in stocks)
            {
                stock.ProfileId = profileid;
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

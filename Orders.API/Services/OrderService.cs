using Orders.API.Models;
using Orders.API.Repositories;
using Microsoft.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;
using Orders.API.Dto;

namespace Orders.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderUpdateRepository _orderUpdateRepository;
        private readonly IHttpClientFactory _httpClientFactory;

        public OrderService(IOrderRepository orderRepository, IOrderUpdateRepository orderUpdateRepository, IHttpClientFactory httpClientFactory)
        {
            _orderRepository = orderRepository;
            _orderUpdateRepository = orderUpdateRepository;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(int profileid, string jwtToken)
        {
            var Orders = await _orderRepository.GetAllOrdersAsync(profileid);
            var latestUpdate = await _orderUpdateRepository.GetLatestUpdateAsync(profileid);

            if (Orders == null || !Orders.Any() || isDataExpired(latestUpdate))
            {
                Orders = await FetchOrdersFromApi(profileid, jwtToken);
                await SaveOrders(Orders, profileid);
                Orders = await _orderRepository.GetAllOrdersAsync(profileid);
            }

            return Orders;
        }

        private bool isDataExpired(OrderUpdate latestUpdate)
        {
            if (latestUpdate.UpdateId < 0)
                return true;
            return DateTime.UtcNow > latestUpdate.LastUpdate.AddMinutes(latestUpdate.LifetimeMinutes);
        }

        private async Task<IEnumerable<Order>> FetchOrdersFromApi(int profileid, string jwtToken)
        {
            var client = _httpClientFactory.CreateClient();
            // Get auth key
            string? authkey = await GetAuthorizationKey(profileid, jwtToken) ?? null;

            if (authkey == null)
                throw new Exception("Authorization key not found.");

            // Add auth key in header params
            client.DefaultRequestHeaders.Add(HeaderNames.Authorization, authkey);

            // Get Orders from WB api
            var responce = await client.GetAsync(OrderConstants.OrdersQuery);
            responce.EnsureSuccessStatusCode();
            var content = await responce.Content.ReadAsStringAsync();
            var apiOrders = JsonSerializer.Deserialize<IEnumerable<ApiOrder>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (apiOrders == null)
                throw new Exception("Failed to fetch Orders.");

            // Map dto to model with counting of repeated elements
            var orders = apiOrders
                .GroupBy(apiOrder => new
                {
                    apiOrder.Date,
                    apiOrder.Subject,
                    apiOrder.Barcode,
                    apiOrder.Brand,
                    apiOrder.Category,
                    apiOrder.TotalPrice,
                    apiOrder.CountryName,
                    apiOrder.OblastOkrugName,
                    apiOrder.RegionName
                })
                .Select(group => new Order
                {
                    ProfileId = profileid,
                    OrderDate = group.Key.Date,
                    OrderName = group.Key.Subject ?? string.Empty,
                    OrderSku = group.Key.Barcode ?? string.Empty,
                    OrderBrand = group.Key.Brand ?? string.Empty,
                    OrderCategory = group.Key.Category ?? string.Empty,
                    OrderAmount = group.Key.TotalPrice * group.Count(),
                    OrderCountry = group.Key.CountryName ?? string.Empty,
                    OrderState = group.Key.OblastOkrugName ?? string.Empty,
                    OrderRegion = group.Key.RegionName ?? string.Empty,
                    OrderQuantity = group.Count()
                })
                .ToList();

            //var orders = apiOrders.Select(apiOrder => new Order
            //{
            //    ProfileId = profileid,
            //    OrderDate = apiOrder.Date,
            //    OrderName = apiOrder.Subject ?? string.Empty,
            //    OrderSku = apiOrder.Barcode ?? string.Empty,
            //    OrderBrand = apiOrder.Brand ?? string.Empty,
            //    OrderCategory = apiOrder.Category ?? string.Empty,
            //    OrderAmount = apiOrder.TotalPrice, 
            //    OrderCountry = apiOrder.CountryName ?? string.Empty,  
            //    OrderState = apiOrder.OblastOkrugName ?? string.Empty,
            //    OrderRegion = apiOrder.RegionName ?? string.Empty
            //}).ToList();
            
            return orders;
        }

        private async Task<string?> GetAuthorizationKey(int profileid, string jwtToken)
        {
            var client = _httpClientFactory.CreateClient();
            var url = OrderConstants.ProfileAuthKeyQuery + profileid;
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

        private async Task SaveOrders(IEnumerable<Order> Orders, int profileid)
        {
            // Delete old Orders
            await _orderRepository.DeleteOrdersByProfileAsync(profileid);
            await _orderUpdateRepository.DeleteOrderUpdatesByProfileAsync(profileid);
            // Add new Orders
            foreach (var Order in Orders)
            {
                Order.OrderId = await _orderRepository.AddOrderAsync(Order);
            }

            // Create OrderUpdate
            var OrderUpdate = new OrderUpdate
            {
                ProfileId = profileid,
                LastUpdate = DateTime.UtcNow,
                LifetimeMinutes = 30,
                DateFrom = new DateTime(2019, 6, 20)
            };

            await _orderUpdateRepository.AddOrderUpdateAsync(OrderUpdate);
        }
    }
}

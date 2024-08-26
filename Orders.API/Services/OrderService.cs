using Orders.API.Models;
using Orders.API.Repositories;
using Microsoft.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;
using Orders.API.Dto;
using System.Collections.Immutable;
using Helpers;


namespace Orders.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderUpdateRepository _orderUpdateRepository;
        private readonly IRequestApiHelper _requestApiHelper;

        public OrderService(IOrderRepository orderRepository, IOrderUpdateRepository orderUpdateRepository, IRequestApiHelper requestApiHelper)
        {
            _orderRepository = orderRepository;
            _orderUpdateRepository = orderUpdateRepository;
            _requestApiHelper = requestApiHelper;
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(int profileid, string jwtToken)
        {
            // Get from database
            var orders = await _orderRepository.GetAllOrdersAsync(profileid);
            var latestUpdate = await _orderUpdateRepository.GetLatestUpdateAsync(profileid);

            if (orders == null || !orders.Any() || CheckDataHelper.IsDataExpired(latestUpdate))
            {
                // Get from api
                var apiOrders = await _requestApiHelper.FetchListFromApi<ApiOrder>(OrderConstants.OrdersQuery, profileid, jwtToken);

                // Map dto to model with counting of repeated elements
                orders = OrderDtoMapper.MapDtoListToModelList(apiOrders);


                // Save in database
                await SaveOrders(orders, profileid);
                orders = await _orderRepository.GetAllOrdersAsync(profileid);
            }

            return orders;
        }


        private async Task SaveOrders(IEnumerable<Order> Orders, int profileid)
        {
            // Delete old Orders
            await _orderRepository.DeleteOrdersByProfileAsync(profileid);
            await _orderUpdateRepository.DeleteOrderUpdatesByProfileAsync(profileid);

            // Add new Orders
            foreach (var Order in Orders)
            {
                Order.ProfileId = profileid;
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

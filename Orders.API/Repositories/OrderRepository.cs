using Orders.API.Database;
using Orders.API.Models;

namespace Orders.API.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        public async Task<IEnumerable<Order>> GetAllOrdersAsync(int profileId)
        {
            const string query = "SELECT * FROM Orders WHERE profile_id = @ProfileId";
            return await DbHelper.QueryAsync<Order>(query, new { ProfileId = profileId });
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            const string query = "SELECT * FROM Orders WHERE order_id = @Id";
            return await DbHelper.QueryFirstOrDefaultAsync<Order>(query, new { Id = id }) ?? new Order();
        }

        public async Task<int> AddOrderAsync(Order order)
        {
            const string query = @"
                INSERT INTO Orders (profile_id, order_date, order_amount, order_quantity, order_name, 
                order_sku, order_brand, order_category, order_country, order_state, order_region) 
                VALUES (@ProfileId, @OrderDate, @OrderAmount, @OrderQuantity, @OrderName, @OrderSku, 
                @OrderBrand, @OrderCategory, @OrderCountry, @OrderState, @OrderRegion)
                RETURNING order_id";
            return await DbHelper.QueryFirstOrDefaultAsync<int>(query, order);
        }

        public async Task<bool> UpdateOrderAsync(Order order)
        {
            const string query = @"
                UPDATE Orders 
                SET profile_id = @ProfileId, order_date = @OrderDate, 
                    order_amount = @OrderAmount, order_quantity = @OrderQuantity 
                    order_name = @OrderName, order_sku = @OrderSku, 
                    order_brand = @OrderBrand, order_category = @OrderCategory, 
                    order_country = @OrderCountry, order_state = @OrderState, 
                    order_region = @OrderRegion
                WHERE order_id = @OrderId";
            int rowsAffected = await DbHelper.ExecuteAsync(query, order);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            const string query = "DELETE FROM Orders WHERE order_id = @OrderId";
            int rowsAffected = await DbHelper.ExecuteAsync(query, new { OrderId = id });
            return rowsAffected > 0;
        }

        public async Task<int> DeleteOrdersByProfileAsync(int profileId)
        {
            const string query = "DELETE FROM Orders WHERE profile_id = @ProfileId";
            int rowsAffected = await DbHelper.ExecuteAsync(query, new { ProfileId = profileId });
            return rowsAffected;
        }
    }


}

using Orders.API.Models;

namespace Orders.API.Dto
{
    public class OrderDtoMapper
    {
        public static IEnumerable<Order> MapDtoListToModelList(IEnumerable<ApiOrder> dtoList)
        {
            return dtoList
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
                });
        }

        public static Order MapDtoToModel(ApiOrder dto)
        {
            throw new NotImplementedException();
        }
    }
}

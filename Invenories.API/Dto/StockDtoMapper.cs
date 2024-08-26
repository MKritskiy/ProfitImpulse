using Inventories.API.Models;

namespace Inventories.API.Dto
{
    public class StockDtoMapper
    {
        public static IEnumerable<Stock> MapDtoListToModelList(IEnumerable<ApiStock> dtoList)
        {
            return dtoList.Select(apiStock => new Stock
            {
                WarehouseName = apiStock.WarehouseName,
                ProductQuantity = apiStock.Quantity,
                ProductName = apiStock.Category,
                ProductSku = apiStock.Barcode,
            });
        }
    }
}

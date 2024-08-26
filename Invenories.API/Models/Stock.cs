namespace Inventories.API.Models
{
    public class Stock
    {
        public int StockId { get; set; }
        public int ProfileId { get; set; }
        public string? WarehouseName { get; set; }
        public int ProductQuantity { get; set; }
        public string? ProductName { get; set; }
        public string? ProductSku { get; set; }
    }
}

namespace Inventories.API.Dto
{
    public class ApiStock
    {
        public string? WarehouseName { get; set; }
        public int Quantity { get; set; }
        public string? Category { get; set; }
        public string? Barcode { get; set; }
    }
}

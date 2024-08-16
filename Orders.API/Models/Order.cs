namespace Orders.API.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int ProfileId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal OrderAmount { get; set; }
        public int OrderQuantity { get; set; } = 1;
        public string OrderName { get; set; } = string.Empty;
        public string OrderSku { get; set; } = string.Empty;
        public string OrderBrand { get; set; } = string.Empty;
        public string OrderCategory { get; set; } = string.Empty;
        public string OrderCountry { get; set; } = string.Empty;
        public string OrderState { get; set; } = string.Empty;
        public string OrderRegion { get; set; } = string.Empty;
    }
}

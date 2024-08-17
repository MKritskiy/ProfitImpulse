namespace Purchases.API.Dto
{
    public class ApiPurchase
    {
        public DateTime Date { get; set; }
        public string? Subject { get; set; }
        public string? Barcode { get; set; }
        public string? Brand { get; set; }
        public string? Category { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; } 
        public decimal FinishedPrice { get; set; }
        public string? CountryName { get; set; }
        public string? OblastOkrugName { get; set; }
        public string? RegionName { get; set; }
    }
}

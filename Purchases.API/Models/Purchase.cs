namespace Purchases.API.Models
{
    public class Purchase
    {
        public int PurchaseId { get; set; }
        public int ProfileId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal PurchaseAmount { get; set; }
        public int PurchaseQuantity { get; set; } = 1;
        public string PurchaseName { get; set; } = string.Empty;
        public string PurchaseSku { get; set; } = string.Empty;
        public string PurchaseBrand { get; set; } = string.Empty;
        public string PurchaseCategory { get; set; } = string.Empty;
        public string PurchaseCountry { get; set; } = string.Empty;
        public string PurchaseState { get; set; } = string.Empty;
        public string PurchaseRegion { get; set; } = string.Empty;
    }
}

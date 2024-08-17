namespace Purchases.API.Models
{
    public class PurchaseUpdate
    {
        public int UpdateId { get; set; }
        public int ProfileId { get; set; }
        public DateTime LastUpdate { get; set; }
        public int LifetimeMinutes { get; set; }
        public DateTime DateFrom { get; set; }
    }
}

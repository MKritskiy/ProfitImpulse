namespace Inventories.API.Models
{
    public class StockUpdate
    {
        public int UpdateId { get; set; }
        public int ProfileId { get; set; }
        public DateTime LastUpdate { get; set; }
        public int LifetimeMinutes { get; set; }
        public DateTime DateFrom { get; set; }
    }
}

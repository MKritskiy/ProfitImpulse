namespace Orders.API.Dto
{
    public class ApiOrder
    {
        public DateTime Date { get; set; }
        public string? Subject { get; set; }
        public string? Barcode { get; set; }
        public string? Brand { get; set; }
        public string? Category { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; } 
        public string? CountryName { get; set; }
        public string? OblastOkrugName { get; set; }
        public string? RegionName { get; set; }
    }
}

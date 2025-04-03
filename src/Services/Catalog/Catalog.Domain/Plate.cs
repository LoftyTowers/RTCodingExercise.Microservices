namespace Catalog.Domain
{
    public class Plate
    {
        public Guid Id { get; set; }

        public string? Registration { get; set; }

        public decimal PurchasePrice { get; set; }

        public decimal SalePrice { get; set; }

        public string? Letters { get; set; }

        public int Numbers { get; set; }

        public decimal? FinalSalePrice { get; set; }

        public string? PromoCodeUsed { get; set; }

        public int StatusId { get; set; } = 10; // FK to "Available"
        

        public Status Status { get; set; }

        public ICollection<AuditLog> AuditLogs { get; set; }
    }
}
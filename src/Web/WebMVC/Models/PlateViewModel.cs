namespace RTCodingExercise.Microservices.Models
{
    public class PlateViewModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Registration { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }
    }
}

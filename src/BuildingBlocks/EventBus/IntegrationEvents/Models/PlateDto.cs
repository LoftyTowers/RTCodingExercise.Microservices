namespace RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models
{
    public class PlateDto
    {
        public Guid Id { get; set; }
        public string Registration { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }
    }
}

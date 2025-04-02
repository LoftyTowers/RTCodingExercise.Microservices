namespace RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models
{
    public class PlateDataDto
    {
        public List<PlateDto> Plates { get; set; }

        public decimal TotalRevenue { get; set; }
        public decimal AverageProfitMargin { get; set; }
    }
}
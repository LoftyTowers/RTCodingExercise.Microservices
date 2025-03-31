namespace RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents
{
    public class ProfitStatsCalculatedEvent : IntegrationEvent
    {
        public decimal TotalRevenue { get; set; }
        public decimal AverageProfitMargin { get; set; }

        public ProfitStatsCalculatedEvent(decimal totalRevenue, decimal averageProfitMargin) : base()
        {
            TotalRevenue = totalRevenue;
            AverageProfitMargin = averageProfitMargin;
        }
    }
}

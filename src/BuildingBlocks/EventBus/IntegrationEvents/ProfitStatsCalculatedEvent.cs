using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;

namespace RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents
{
    public class ProfitStatsCalculatedEvent : IntegrationEvent
    {

        public ProfitStatsDto Stats { get; set; }

        public ProfitStatsCalculatedEvent(ProfitStatsDto stats) : base()
        {
            Stats = stats;
        }
    }
}

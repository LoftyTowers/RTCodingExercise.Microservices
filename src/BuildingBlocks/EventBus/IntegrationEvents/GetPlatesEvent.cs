using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;
using EventBus.Enums;

namespace RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents
{
    public class GetPlatesEvent : IntegrationEvent
    {
        public SortField SortField { get; set; } = SortField.None;
        public SortDirection SortDirection { get; set; } = SortDirection.Ascending;
        public string Filter { get; set; } = string.Empty;
        public bool OnlyAvailable { get; set; } = false;

        public GetPlatesEvent() : base()
        {
            CorrelationId = Guid.NewGuid();
        }
    }
}
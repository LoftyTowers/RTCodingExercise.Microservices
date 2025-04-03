using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;

namespace RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents
{
    public class PlateStatusUpdateCompletedEvent : IntegrationEvent
    {
        public PlateDataDto PlateData { get; set; }

        public PlateStatusUpdateCompletedEvent(PlateDataDto plateData) : base()
        {
            PlateData = plateData;
        }
    }
}

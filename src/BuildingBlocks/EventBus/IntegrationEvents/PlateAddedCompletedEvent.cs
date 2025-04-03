using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;

namespace RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents
{
    public class PlateAddedCompletedEvent : IntegrationEvent
    {
        public PlateDataDto PlateData { get; set; }

        public PlateAddedCompletedEvent(PlateDataDto plateData) : base()
        {
            PlateData = plateData;
        }
    }
}

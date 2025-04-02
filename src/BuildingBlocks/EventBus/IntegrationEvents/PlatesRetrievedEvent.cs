using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;

namespace RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents
{
    public class PlatesRetrievedEvent : IntegrationEvent
    {
        public PlateDataDto PlateData { get; set; }

        public PlatesRetrievedEvent(PlateDataDto plateData) : base()
        {
            PlateData = plateData;
        }
    }
}

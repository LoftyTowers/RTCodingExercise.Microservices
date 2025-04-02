using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;

namespace RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents
{
    public class PlatesRetrievedEvent : IntegrationEvent
    {
        public PlateDataDto Plates { get; set; }

        public PlatesRetrievedEvent(PlateDataDto plates) : base()
        {
            Plates = plates;
        }
    }
}

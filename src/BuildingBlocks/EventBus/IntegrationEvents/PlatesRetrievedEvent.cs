using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;

namespace RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents
{
    public class PlatesRetrievedEvent : IntegrationEvent
    {
        public List<PlateDto> Plates { get; set; }

        public PlatesRetrievedEvent(List<PlateDto> plates) : base()
        {
            Plates = plates;
        }
    }
}

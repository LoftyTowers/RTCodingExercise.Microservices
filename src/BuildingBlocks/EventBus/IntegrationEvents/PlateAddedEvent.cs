using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;

namespace RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents
{
    public class PlateAddedEvent : IntegrationEvent
    {
        public PlateAddedEvent() : base()
        {

        }

        public PlateDto Plate { get; set; }
    }
}
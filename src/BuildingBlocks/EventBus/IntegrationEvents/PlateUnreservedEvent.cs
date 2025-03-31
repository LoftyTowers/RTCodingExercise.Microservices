using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;

namespace RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents
{
    public class PlateUnreservedEvent : IntegrationEvent
    {
        public PlateDto Plate { get; set; }

        public PlateUnreservedEvent(PlateDto plate) : base()
        {
            Plate = plate;
        }
    }
}

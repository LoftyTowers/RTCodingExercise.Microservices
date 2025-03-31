using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;

namespace RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents
{
    public class PlateReservedEvent : IntegrationEvent
    {
        public PlateDto Plate { get; set; }

        public PlateReservedEvent(PlateDto plate) : base()
        {
            Plate = plate;
        }
    }
}

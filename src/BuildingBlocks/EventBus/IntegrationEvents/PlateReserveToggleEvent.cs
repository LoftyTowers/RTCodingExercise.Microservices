using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;

namespace RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents
{
    public class PlateReserveToggleEvent : IntegrationEvent
    {
        public PlateDto Plate { get; set; }

        public PlateReserveToggleEvent(PlateDto plate) : base()
        {
            Plate = plate;
        }
    }
}

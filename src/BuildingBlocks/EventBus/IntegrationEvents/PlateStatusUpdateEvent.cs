using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;

namespace RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents
{
    public class PlateStatusUpdateEvent : IntegrationEvent
    {
        public PlateDto Plate { get; set; }

        public PlateStatusUpdateEvent(PlateDto plate) : base()
        {
            Plate = plate;
        }
    }
}

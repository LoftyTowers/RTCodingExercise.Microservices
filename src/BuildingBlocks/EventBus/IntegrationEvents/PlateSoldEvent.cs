using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;

namespace RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents
{
    public class PlateSoldEvent : IntegrationEvent
    {
        public PlateDto Plate { get; set; }

        public PlateSoldEvent(PlateDto plate) : base()
        {
            Plate = plate;
        }
    }
}

using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;

namespace RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents
{
    public class SellPlateEvent : IntegrationEvent
    {
        public PlateDto Plate { get; set; }

        public SellPlateEvent(PlateDto plate) : base()
        {
            Plate = plate;
        }
    }
}

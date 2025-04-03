using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;

namespace RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents
{
    public class SellPlateCompletedEvent : IntegrationEvent
    {
        public PlateDataDto PlateData { get; set; }

        public SellPlateCompletedEvent(PlateDataDto plateData) : base()
        {
            PlateData = plateData;
        }
    }
}

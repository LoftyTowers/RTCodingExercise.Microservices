namespace RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents
{
    public class ApplyPercentOffEvent : IntegrationEvent
    {
        public string PromoCode { get; set; }

        public ApplyPercentOffEvent(string promoCode) : base()
        {
            PromoCode = promoCode;
        }
    }
}

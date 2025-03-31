namespace RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents
{
    public class ApplyDiscountEvent : IntegrationEvent
    {
        public string PromoCode { get; set; }

        public ApplyDiscountEvent(string promoCode) : base()
        {
            PromoCode = promoCode;
        }
    }
}

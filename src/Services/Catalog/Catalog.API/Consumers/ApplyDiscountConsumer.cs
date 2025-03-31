using MassTransit;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;
using Catalog.API.Services;
using AutoMapper;

namespace Catalog.API.Consumers
{
    public class ApplyDiscountConsumer : IConsumer<ApplyDiscountEvent>
    {
        private readonly IPromotionService _promotionService;
        private readonly ILogger<ApplyDiscountConsumer> _logger;

        public ApplyDiscountConsumer(IPromotionService promotionService, ILogger<ApplyDiscountConsumer> logger)
        {
            _promotionService = promotionService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ApplyDiscountEvent> context)
        {
            try
            {
                await _promotionService.ApplyDiscountAsync(context.Message.PromoCode);
                _logger.LogInformation("Processed ApplyDiscountEvent with promo code: {PromoCode}", context.Message.PromoCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing ApplyDiscountEvent for promo code: {PromoCode}", context.Message.PromoCode);
                throw;
            }
        }
    }
}
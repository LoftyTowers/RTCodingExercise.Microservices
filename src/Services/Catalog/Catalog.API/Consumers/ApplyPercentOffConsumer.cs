using MassTransit;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;
using Catalog.API.Services;
using AutoMapper;

namespace Catalog.API.Consumers
{
    public class ApplyPercentOffConsumer : IConsumer<ApplyPercentOffEvent>
    {
        private readonly IPromotionService _promotionService;
        private readonly ILogger<ApplyPercentOffConsumer> _logger;

        public ApplyPercentOffConsumer(IPromotionService promotionService, ILogger<ApplyPercentOffConsumer> logger)
        {
            _promotionService = promotionService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ApplyPercentOffEvent> context)
        {
            try
            {
                await _promotionService.ApplyPercentOffAsync(context.Message.PromoCode);
                _logger.LogInformation("Processed ApplyPercentOffEvent with promo code: {PromoCode}", context.Message.PromoCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing ApplyPercentOffEvent for promo code: {PromoCode}", context.Message.PromoCode);
                throw;
            }
        }
    }
}
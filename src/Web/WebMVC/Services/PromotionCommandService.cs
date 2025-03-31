using AutoMapper;
using MassTransit;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents;
using RTCodingExercise.Microservices.Models;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;

namespace RTCodingExercise.Microservices.WebMVC.Services
{
    public class PromotionCommandService : IPromotionCommandService
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;
        private readonly ILogger<PromotionCommandService> _logger;

        public PromotionCommandService(IPublishEndpoint publishEndpoint, IMapper mapper, ILogger<PromotionCommandService> logger)
        {
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task ApplyDiscountAsync(string promoCode)
        {
            await _publishEndpoint.Publish(new ApplyDiscountEvent(promoCode));
        }

        public async Task ApplyPercentOffAsync(string promoCode)
        {
            await _publishEndpoint.Publish(new ApplyPercentOffEvent(promoCode));
        }
    }
}
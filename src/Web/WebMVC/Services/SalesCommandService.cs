using AutoMapper;
using MassTransit;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents;
using RTCodingExercise.Microservices.Models;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;

namespace RTCodingExercise.Microservices.WebMVC.Services
{
    public class SalesCommandService : ISalesCommandService
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;
        private readonly ILogger<SalesCommandService> _logger;

        public SalesCommandService(IPublishEndpoint publishEndpoint, IMapper mapper, ILogger<SalesCommandService> logger)
        {
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task SellPlate(PlateViewModel plate)
        {
            try
            {
                var dto = _mapper.Map<PlateDto>(plate);
                var @event = new SellPlateEvent(dto);

                _logger.LogInformation("Publishing SellPlateEvent for plate {Id} with final sale price {Price} and promo code {Promo}.", dto.Id, dto.FinalSalePrice, dto.PromoCodeUsed);

                await _publishEndpoint.Publish(@event);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to publish SellPlateEvent for plate ID: {Id}", plate.Id);
                throw;
            }
        }
    }
}
using MassTransit;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;
using Catalog.API.Services;
using AutoMapper;

namespace Catalog.API.Consumers
{
    public class SellPlateConsumer : IConsumer<SellPlateEvent>
    {
        private readonly IPlateService _plateService;
        private readonly IMapper _mapper;
        private readonly ILogger<SellPlateConsumer> _logger;

        public SellPlateConsumer(IPlateService plateService, IMapper mapper, ILogger<SellPlateConsumer> logger)
        {
            _plateService = plateService;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<SellPlateEvent> context)
        {
            var dto = context.Message.Plate;

            _logger.LogInformation("SellPlateEvent received: ID={Id}, Promo={Promo}, FinalPrice={Price}",
                dto.Id, dto.PromoCodeUsed, dto.FinalSalePrice);

            try
            {
                var plate = _mapper.Map<Plate>(dto);

                var plateDtos = await _plateService.SellPlateAsync(plate);

                _logger.LogInformation("SellPlateEvent processed successfully for ID={Id}", plate.Id);
                
                var response = new SellPlateCompletedEvent(plateDtos)
                {
                    CorrelationId = context.CorrelationId ?? Guid.NewGuid()
                };

                _logger.LogInformation("Returning plate data with CorrelationId {CorrelationId}", response.CorrelationId);
                await context.RespondAsync(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing SellPlateEvent for Plate ID={Id}", dto.Id);
                throw;
            }
        }
    }

}
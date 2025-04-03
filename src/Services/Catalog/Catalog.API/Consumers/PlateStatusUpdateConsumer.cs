using MassTransit;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;
using Catalog.API.Services;
using AutoMapper;

namespace Catalog.API.Consumers
{
    public class PlateStatusUpdateConsumer : IConsumer<PlateStatusUpdateEvent>
    {
        private readonly IPlateService _plateService;
        private readonly ILogger<PlateStatusUpdateConsumer> _logger;
        private readonly IMapper _mapper;

        public PlateStatusUpdateConsumer(IPlateService plateService, ILogger<PlateStatusUpdateConsumer> logger, IMapper mapper)
        {
            _plateService = plateService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<PlateStatusUpdateEvent> context)
        {
            try
            {
                var plateDtos = await _plateService.UpdateStatusAsync(_mapper.Map<Plate>(context.Message.Plate));
                _logger.LogInformation("Processed PlateReservedEvent for Plate ID: {PlateId}", context.Message.Plate.Id);
                
                var response = new PlateStatusUpdateCompletedEvent(plateDtos)
                {
                    CorrelationId = context.CorrelationId ?? Guid.NewGuid()
                };

                _logger.LogInformation("Returning plate data with CorrelationId {CorrelationId}", response.CorrelationId);
                await context.RespondAsync(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing PlateReservedEvent for Plate ID: {PlateId}", context.Message.Plate.Id);
                throw;
            }
        }
    }
}
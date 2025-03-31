using MassTransit;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;
using Catalog.API.Services;
using AutoMapper;

namespace Catalog.API.Consumers
{
    public class PlateSoldConsumer : IConsumer<PlateSoldEvent>
    {
        private readonly ISalesService _salesService;
        private readonly ILogger<PlateSoldConsumer> _logger;

        public PlateSoldConsumer(ISalesService salesService, ILogger<PlateSoldConsumer> logger)
        {
            _salesService = salesService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<PlateSoldEvent> context)
        {
            try
            {
                await _salesService.SellPlateAsync(context.Message.Plate.Id);
                _logger.LogInformation("Processed PlateSoldEvent for Plate ID: {PlateId}", context.Message.Plate.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing PlateSoldEvent for Plate ID: {PlateId}", context.Message.Plate.Id);
                throw;
            }
        }
    }
}
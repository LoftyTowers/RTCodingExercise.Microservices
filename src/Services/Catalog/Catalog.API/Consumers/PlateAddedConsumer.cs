using MassTransit;
using Catalog.API.Services;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents;

namespace Catalog.API.Consumers
{
    public class PlateAddedConsumer : IConsumer<PlateAddedEvent>
    {
        private readonly ILogger<PlateAddedConsumer> _logger;
        private readonly IPlateService _plateService;

        public PlateAddedConsumer(ILogger<PlateAddedConsumer> logger, IPlateService plateService)
        {
            _logger = logger;
            _plateService = plateService;
        }

        public async Task Consume(ConsumeContext<PlateAddedEvent> context)
        {
            _logger.LogInformation($"Received PlateAddedEvent for plate {context.Message.Plate.Registration}");

            try
            {
                _logger.LogInformation("Received PlateAddedEvent for plate: {Plate}", context.Message.Plate.Registration);
                await _plateService.AddPlateAsync(context.Message.Plate);
                _logger.LogInformation("Successfully added plate: {Plate}", context.Message.Plate.Registration);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while processing PlateAddedEvent for plate {context.Message.Plate.Registration}");
                throw; // Rethrow the exception to ensure MassTransit can handle it (e.g., retry, dead-letter, etc.)
            }
        }
    }
}

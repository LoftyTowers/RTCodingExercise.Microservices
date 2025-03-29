using MassTransit;
using Microsoft.Extensions.Logging;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents;

namespace Catalog.API.Consumers
{
    public class PlateAddedConsumer : IConsumer<PlateAddedEvent>
    {
        private readonly ILogger<PlateAddedConsumer> _logger;

        public PlateAddedConsumer(ILogger<PlateAddedConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<PlateAddedEvent> context)
        {
            _logger.LogInformation($"Received PlateAddedEvent for plate {context.Message.Plate.Registration}");

            // Simulate processing the added plate (save to DB, etc.)
            await Task.CompletedTask;
        }
    }
}

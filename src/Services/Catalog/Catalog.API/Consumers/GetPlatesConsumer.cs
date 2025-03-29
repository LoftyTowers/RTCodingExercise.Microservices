
using MassTransit;
using Microsoft.Extensions.Logging;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;

namespace Catalog.API.Consumers
{
    public class GetPlatesConsumer : IConsumer<GetPlatesEvent>
    {
        private readonly ILogger<GetPlatesConsumer> _logger;

        public GetPlatesConsumer(ILogger<GetPlatesConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<GetPlatesEvent> context)
        {
            _logger.LogInformation("Received GetPlatesEvent");

            // Stub response (replace with actual data fetch later)
            var response = new PlatesRetrievedEvent(new List<PlateDto>
            {
                new PlateDto { Id = Guid.NewGuid(), Registration = "ABC 123", PurchasePrice = 500, SalePrice = 600 },
                new PlateDto { Id = Guid.NewGuid(), Registration = "XYZ 789", PurchasePrice = 700, SalePrice = 840 },
            });


            _logger.LogInformation("Returning plate data in response");
            await context.RespondAsync(response);
        }
    }
}

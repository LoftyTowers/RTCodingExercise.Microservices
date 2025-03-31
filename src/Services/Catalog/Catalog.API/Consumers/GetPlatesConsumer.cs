using MassTransit;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;
using Catalog.API.Repositories;
using AutoMapper;

namespace Catalog.API.Consumers
{
     public class GetPlatesConsumer : IConsumer<GetPlatesEvent>
    {
        private readonly ILogger<GetPlatesConsumer> _logger;
        private readonly IPlateRepository _plateRepository;
        private readonly IMapper _mapper;

        public GetPlatesConsumer(ILogger<GetPlatesConsumer> logger, IPlateRepository plateRepository, IMapper mapper)
        {
            _logger = logger;
            _plateRepository = plateRepository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<GetPlatesEvent> context)
        {
            try
            {
                _logger.LogInformation("Received GetPlatesEvent with CorrelationId {CorrelationId}", context.CorrelationId);

                var plates = await _plateRepository.GetAllPlatesAsync();

                if (plates == null || !plates.Any())
                {
                    _logger.LogWarning("No plates found in the repository.");
                    // You might want to respond with an empty list anyway, or throw depending on your use case
                }

                var plateDtos = _mapper.Map<List<PlateDto>>(plates);

                var response = new PlatesRetrievedEvent(plateDtos)
                {
                    CorrelationId = context.CorrelationId ?? Guid.NewGuid()
                };

                _logger.LogInformation("Returning plate data with CorrelationId {CorrelationId}", response.CorrelationId);
                await context.RespondAsync(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception while processing GetPlatesEvent.");
                await context.RespondAsync<Fault<GetPlatesEvent>>(ex);
            }
        }
    }
}

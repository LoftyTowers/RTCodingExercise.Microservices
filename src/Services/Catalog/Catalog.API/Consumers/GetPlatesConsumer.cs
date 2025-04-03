using MassTransit;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;
using Catalog.API.Services;
using Catalog.Domain.Enums;
using AutoMapper;

namespace Catalog.API.Consumers
{
     public class GetPlatesConsumer : IConsumer<GetPlatesEvent>
    {
        private readonly ILogger<GetPlatesConsumer> _logger;
        private readonly IPlateService _plateService;
        private readonly IMapper _mapper;

        public GetPlatesConsumer(ILogger<GetPlatesConsumer> logger, IPlateService plateService, IMapper mapper)
        {
            _logger = logger;
            _plateService = plateService;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<GetPlatesEvent> context)
        {
            try
            {
                _logger.LogInformation("Received GetPlatesEvent with CorrelationId {CorrelationId}", context.CorrelationId);
                var plateDtos = await _plateService.GetPlatesAsync(_mapper.Map<SortField>(context?.Message?.SortField), _mapper.Map<SortDirection>(context?.Message?.SortDirection), context?.Message.Filter, context?.Message?.OnlyAvailable);

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

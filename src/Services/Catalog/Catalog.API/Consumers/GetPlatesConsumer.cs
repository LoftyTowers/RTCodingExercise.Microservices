using MassTransit;
using Microsoft.Extensions.Logging;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;
using Catalog.Domain;
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
            _logger.LogInformation("Received GetPlatesEvent");

            var plates = await _plateRepository.GetAllPlatesAsync();
            var plateDtos = _mapper.Map<List<PlateDto>>(plates);

            var response = new PlatesRetrievedEvent(plateDtos);

            _logger.LogInformation("Returning plate data in response");
            await context.RespondAsync(response);
        }
    }
}

using MassTransit;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;
using Catalog.API.Services;
using AutoMapper;

namespace Catalog.API.Consumers
{ 
 using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents;

    public class GetProfitStatsConsumer : IConsumer<GetProfitStatsEvent>
    {
        // private readonly ISalesService _salesService;
        // private readonly ILogger<GetProfitStatsConsumer> _logger;
        // private readonly IMapper _mapper;

        // public GetProfitStatsConsumer(ISalesService salesService, ILogger<GetProfitStatsConsumer> logger, IMapper mapper)
        // {
        //     _salesService = salesService;
        //     _logger = logger;
        //     _mapper = mapper;
        // }

        public async Task Consume(ConsumeContext<GetProfitStatsEvent> context)
        {
            // try
            // {
            //     var stats = await _salesService.CalculateProfitStatsAsync();
            //     await context.RespondAsync(new ProfitStatsCalculatedEvent(_mapper.Map<ProfitStatsDto>(stats))
            //     {
            //         CorrelationId = context.CorrelationId ?? Guid.NewGuid()
            //     });

            //     _logger.LogInformation("Responded to GetProfitStatsEvent with revenue: £{Revenue}, margin: {Margin:P2}",
            //         stats.TotalRevenue, stats.AverageProfitMargin);
            // }
            // catch (Exception ex)
            // {
            //     _logger.LogError(ex, "Error responding to GetProfitStatsEvent.");
            //     throw;
            // }
        }
    }   
}
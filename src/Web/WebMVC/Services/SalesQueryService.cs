using AutoMapper;
using MassTransit;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents;
using RTCodingExercise.Microservices.Models;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;

namespace RTCodingExercise.Microservices.WebMVC.Services
{
    public class SalesQueryService : ISalesQueryService
    {
        #region fields

        private readonly IRequestClient<GetProfitStatsEvent> _client;
        private readonly IMapper _mapper;
        private readonly ILogger<SalesQueryService> _logger;

        #endregion

        public SalesQueryService(IRequestClient<GetProfitStatsEvent> client, IMapper mapper, ILogger<SalesQueryService> logger)
        {
            _client = client;
            _mapper = mapper;
            _logger = logger;
        }

        private Task<ProfitStatsViewModel> MapSales(Response<ProfitStatsCalculatedEvent> response)
        {
            if (response.Message.Stats == null)
            {
                _logger.LogWarning("No sales stats found in the response.");
                return Task.FromResult<ProfitStatsViewModel>(null);
            }

            var viewModel = _mapper.Map<ProfitStatsViewModel>(response.Message.Stats);
            return Task.FromResult(viewModel);
        }

        public async Task<ProfitStatsViewModel> GetProfitStatsAsync()
        {
            try
            {
                _logger.LogInformation("Requesting sales from the event bus.");

                var response = await _client.GetResponse<ProfitStatsCalculatedEvent>(new GetProfitStatsEvent
                {
                    CorrelationId = Guid.NewGuid()
                });
                return await MapSales(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception while retrieving sales.");
                throw;
            }
        }
    }
}
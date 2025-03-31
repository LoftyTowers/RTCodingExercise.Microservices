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

        private readonly IRequestClient<GetSalesEvent> _client;
        private readonly IMapper _mapper;
        private readonly ILogger<SalesQueryService> _logger;

        #endregion

        public SalesQueryService(IRequestClient<PlateViewModel> client, IMapper mapper, ILogger<SalesQueryService> logger)
        {
            _client = client;
            _mapper = mapper;
            _logger = logger;
        }

        private List<PlateViewModel> MapSales(Response<PlateViewModel> response)
        {
            if (response.Message.Sales == null || !response.Message.Sales.Any())
            {
                _logger.LogWarning("No sales found in the response.");
                return new List<PlateViewModel>();
            }

            var sales = _mapper.Map<List<PlateViewModel>>(response.Message.Sales);
            _logger.LogInformation($"Mapped {sales.Count} sales successfully.");
            return sales;
        }

        public async Task<ProfitStatsViewModel> GetProfitStatsAsync()
        {
            try
            {
                _logger.LogInformation("Requesting sales from the event bus.");

                var response = await _client.GetResponse<SalesRetrievedEvent>(new GetSalesEvent
                {
                    CorrelationId = Guid.NewGuid()
                });
                return MapSales(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception while retrieving sales.");
                throw;
            }
        }
    }
}
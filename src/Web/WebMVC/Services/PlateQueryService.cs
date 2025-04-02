using AutoMapper;
using MassTransit;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents;
using RTCodingExercise.Microservices.Models;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;
using WebMVC.Enums;

namespace RTCodingExercise.Microservices.WebMVC.Services
{
    public class PlateQueryService : IPlateQueryService
    {
        #region fields

        private readonly IRequestClient<GetPlatesEvent> _client;
        private readonly IMapper _mapper;
        private readonly ILogger<PlateQueryService> _logger;

        #endregion

        public PlateQueryService(IRequestClient<GetPlatesEvent> client, IMapper mapper, ILogger<PlateQueryService> logger)
        {
            _client = client;
            _mapper = mapper;
            _logger = logger;
        }

        private PlateDataViewModel MapPlates(Response<PlatesRetrievedEvent> response)
        {
            if (response.Message.Plates == null || !response.Message.Plates.Any())
            {
                _logger.LogWarning("No plates found in the response.");
                return new PlateDataViewModel();
            }

            var plateData = _mapper.Map<PlateDataViewModel>(response.Message.Plates);
            _logger.LogInformation($"Mapped {plateData.Plates.Count} plates successfully.");
            return plateData;
        }

        public async Task<PlateDataViewModel> GetSortedPlatesAsync(SortField field, SortDirection direction)
        {
            try
            {
                _logger.LogInformation("Requesting plates from the event bus.");

                var response = await _client.GetResponse<PlatesRetrievedEvent>(new GetPlatesEvent
                {
                    SortField = _mapper.Map<EventBus.Enums.SortField>(field),
                    SortDirection = _mapper.Map<EventBus.Enums.SortDirection>(direction),
                    CorrelationId = Guid.NewGuid()
                });
                return MapPlates(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception while retrieving plates.");
                throw;
            }
        }

        public async Task<PlateDataViewModel> FilterPlatesAsync(string filter, bool onlyAvailable)
        {
            try
            {
                _logger.LogInformation("Requesting filtered plates from the event bus.");

                var response = await _client.GetResponse<PlatesRetrievedEvent>(new GetPlatesEvent
                {
                    Filter = filter,
                    OnlyAvailable = onlyAvailable,
                    CorrelationId = Guid.NewGuid()
                });
                return MapPlates(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception while retrieving filtered plates.");
                throw;
            }
        }
    }
}

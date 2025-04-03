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

        private readonly IMapper _mapper;
        private readonly ILogger<PlateQueryService> _logger;
        private readonly IRequestClient<GetPlatesEvent> _getPlatesClient;
        private readonly IRequestClient<PlateAddedEvent> _addPlateClient;
        private readonly IRequestClient<PlateStatusUpdateEvent> _updateStatusClient;

        #endregion

        public PlateQueryService(
            IRequestClient<GetPlatesEvent> getPlatesClient,
            IRequestClient<PlateAddedEvent> addPlateClient,
            IRequestClient<PlateStatusUpdateEvent> updateStatusClient,
            IMapper mapper,
            ILogger<PlateQueryService> logger)
        {
            _getPlatesClient = getPlatesClient;
            _addPlateClient = addPlateClient;
            _updateStatusClient = updateStatusClient;
            _mapper = mapper;
            _logger = logger;
        }

        private PlateDataViewModel MapPlates(PlateDataDto plateDataDto)
        {
            var plateData = _mapper.Map<PlateDataViewModel>(plateDataDto);
            _logger.LogInformation($"Mapped {plateData.Plates.Count} plates successfully.");
            return plateData;
        }

        public async Task<PlateDataViewModel> GetSortedPlatesAsync(SortField field, SortDirection direction)
        {
            try
            {
                _logger.LogInformation("Requesting plates from the event bus.");

                var response = await _getPlatesClient.GetResponse<PlatesRetrievedEvent>(new GetPlatesEvent
                {
                    SortField = _mapper.Map<EventBus.Enums.SortField>(field),
                    SortDirection = _mapper.Map<EventBus.Enums.SortDirection>(direction),
                    CorrelationId = Guid.NewGuid()
                });

                if ((response?.Message?.PlateData == null) || !(response.Message.PlateData.Plates?.Any() ?? false))
                {
                    _logger.LogWarning("No plates found in the response.");
                    return new PlateDataViewModel
                    {
                        Plates = new List<PlateViewModel>(),
                        TotalRevenue = 0,
                        AverageProfitMargin = 0
                    };
                }

                return MapPlates(response.Message.PlateData);
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

                var response = await _getPlatesClient.GetResponse<PlatesRetrievedEvent>(new GetPlatesEvent
                {
                    Filter = filter,
                    OnlyAvailable = onlyAvailable,
                    CorrelationId = Guid.NewGuid()
                });

                if ((response?.Message?.PlateData == null) || !(response.Message.PlateData.Plates?.Any() ?? false))
                {
                    _logger.LogWarning("No plates found in the response.");
                    return new PlateDataViewModel
                    {
                        Plates = new List<PlateViewModel>(),
                        TotalRevenue = 0,
                        AverageProfitMargin = 0
                    };
                }

                return MapPlates(response.Message.PlateData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception while retrieving filtered plates.");
                throw;
            }
        }

        public async Task<PlateDataViewModel> AddPlateAsync(PlateViewModel plate)
        {
            try
            {
                if (plate == null) throw new ArgumentNullException(nameof(plate));
                if (string.IsNullOrWhiteSpace(plate.Registration))
                    throw new ArgumentException("Plate registration cannot be null or empty.", nameof(plate.Registration));

                var plateDto = _mapper.Map<PlateDto>(plate);

                var response = await _addPlateClient.GetResponse<PlateAddedCompletedEvent>(new PlateAddedEvent
                {
                    Plate = plateDto,
                    CorrelationId = Guid.NewGuid()
                });

                if ((response?.Message?.PlateData == null) || !(response.Message.PlateData.Plates?.Any() ?? false))
                {
                    _logger.LogWarning("No plates found in the response.");
                    return new PlateDataViewModel();
                }

                return MapPlates(response.Message.PlateData);
            }
            catch (RequestTimeoutException ex)
            {
                _logger.LogError(ex, "Timeout occurred while sending PlateAddedEvent.");
                throw new ApplicationException("The catalog service did not respond in time.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send PlateAddedEvent.");
                throw new ApplicationException("An error occurred while processing your request.");
            }
        }

        public async Task<PlateDataViewModel> UpdateStatusAsync(PlateViewModel plate)
        {
            try
            {
                if (plate == null)
                    throw new ArgumentNullException(nameof(plate));

                if (plate.Id == Guid.Empty)
                    throw new ArgumentException("Plate ID cannot be empty.", nameof(plate.Id));

                _logger.LogInformation("Toggling reservation for plate: {@Plate}", plate);

                var plateDto = _mapper.Map<PlateDto>(plate);

                var response = await _updateStatusClient.GetResponse<PlateStatusUpdateCompletedEvent>(new PlateStatusUpdateEvent(plateDto)
                {
                    CorrelationId = Guid.NewGuid()
                });

                if ((response?.Message?.PlateData == null) || !(response.Message.PlateData.Plates?.Any() ?? false))
                {
                    _logger.LogWarning("No plates found in the response.");
                    return new PlateDataViewModel();
                }
                
                _logger.LogInformation("Reservation toggled. Plate ID: {PlateId}, Status: {Status}", plate.Id, plate.Status);

                return MapPlates(response.Message.PlateData);

            }
            catch (RequestTimeoutException ex)
            {
                _logger.LogError(ex, "Timeout occurred while publishing reservation toggle for plate ID: {PlateId}", plate?.Id);
                throw new ApplicationException("The catalog service did not respond in time.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while toggling reservation for plate ID: {PlateId}", plate?.Id);
                throw new ApplicationException("An error occurred while processing your request.");
            }
        }
    }
}

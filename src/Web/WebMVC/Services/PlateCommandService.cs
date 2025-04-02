using AutoMapper;
using MassTransit;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents;
using RTCodingExercise.Microservices.Models;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;

namespace RTCodingExercise.Microservices.WebMVC.Services
{
    public class PlateCommandService : IPlateCommandService
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;
        private readonly ILogger<PlateCommandService> _logger;

        public PlateCommandService(IPublishEndpoint publishEndpoint, IMapper mapper, ILogger<PlateCommandService> logger)
        {
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task AddPlateAsync(PlateViewModel plate)
        {
            try
            {
                if (plate == null) throw new ArgumentNullException(nameof(plate));
                if (string.IsNullOrWhiteSpace(plate.Registration))
                    throw new ArgumentException("Plate registration cannot be null or empty.", nameof(plate.Registration));

                var plateDto = _mapper.Map<PlateDto>(plate);
                var eventMessage = new PlateAddedEvent
                {
                    Plate = plateDto,
                    CorrelationId = Guid.NewGuid()
                };
                await _publishEndpoint.Publish(eventMessage);
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

        public async Task UpdateStatusAsync(PlateViewModel plate)
        {
            try
            {
                if (plate == null)
                    throw new ArgumentNullException(nameof(plate));

                if (plate.Id == Guid.Empty)
                    throw new ArgumentException("Plate ID cannot be empty.", nameof(plate.Id));

                _logger.LogInformation("Toggling reservation for plate: {@Plate}", plate);

                var plateDto = _mapper.Map<PlateDto>(plate);

                var toggleEvent = new PlateStatusUpdateEvent(plateDto)
                {
                    CorrelationId = Guid.NewGuid()
                };

                await _publishEndpoint.Publish(toggleEvent);

                _logger.LogInformation("Reservation toggled. Plate ID: {PlateId}, Status: {Status}", plate.Id, plate.Status);
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
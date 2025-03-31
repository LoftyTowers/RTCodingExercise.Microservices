using AutoMapper;
using MassTransit;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents;
using RTCodingExercise.Microservices.Models;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;

namespace RTCodingExercise.Microservices.WebMVC.Services
{
    public class ReservationCommandService : IReservationCommandService
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;
        private readonly ILogger<ReservationCommandService> _logger;

        public ReservationCommandService(IPublishEndpoint publishEndpoint, IMapper mapper, ILogger<ReservationCommandService> logger)
        {
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task ReservePlateAsync(PlateViewModel plate)
        {
            try
            {
                if (plate == null) throw new ArgumentNullException(nameof(plate));
                if (string.IsNullOrWhiteSpace(plate.Id.ToString()))
                    throw new ArgumentException("Reservation plate ID cannot be null or empty.", nameof(plate.Id));

                var reservationDto = _mapper.Map<PlateDto>(plate);
                var eventMessage = new PlateReservedEvent(reservationDto)
                {
                    Plate = reservationDto,
                    CorrelationId = Guid.NewGuid()
                };
                await _publishEndpoint.Publish(eventMessage);
            }
            catch (RequestTimeoutException ex)
            {
                _logger.LogError(ex, "Timeout occurred while sending ReservationAddedEvent.");
                throw new ApplicationException("The catalog service did not respond in time.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send ReservationAddedEvent.");
                throw new ApplicationException("An error occurred while processing your request.");
            }
        }

        public async Task UnreservePlateAsync(PlateViewModel plate)
        {
            try
            {
                if (plate == null) throw new ArgumentNullException(nameof(plate));
                if (string.IsNullOrWhiteSpace(plate.Id.ToString()))
                    throw new ArgumentException("Unreservation plate ID cannot be null or empty.", nameof(plate.Id));

                var reservationDto = _mapper.Map<PlateDto>(plate);
                var eventMessage = new PlateUnreservedEvent(reservationDto)
                {
                    CorrelationId = Guid.NewGuid()
                };
                await _publishEndpoint.Publish(eventMessage);
            }
            catch (RequestTimeoutException ex)
            {
                _logger.LogError(ex, "Timeout occurred while sending ReservationRemovedEvent.");
                throw new ApplicationException("The catalog service did not respond in time.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send ReservationRemovedEvent.");
                throw new ApplicationException("An error occurred while processing your request.");
            }
        }
    }
}
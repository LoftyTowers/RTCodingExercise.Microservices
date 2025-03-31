using AutoMapper;
using MassTransit;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents;
using RTCodingExercise.Microservices.Models;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;

namespace RTCodingExercise.Microservices.WebMVC.Services
{
    public class SalesCommandService : ISalesCommandService
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;
        private readonly ILogger<SalesCommandService> _logger;

        public SalesCommandService(IPublishEndpoint publishEndpoint, IMapper mapper, ILogger<SalesCommandService> logger)
        {
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task SellPlateAsync(PlateViewModel Plate)
        {
            try
            {
                if (sale == null) throw new ArgumentNullException(nameof(sale));
                if (string.IsNullOrWhiteSpace(sale.PlateId.ToString()))
                    throw new ArgumentException("Sale plate ID cannot be null or empty.", nameof(sale.PlateId));

                var saleDto = _mapper.Map<SaleDto>(sale);
                var eventMessage = new SaleAddedEvent
                {
                    Sale = saleDto,
                    CorrelationId = Guid.NewGuid()
                };
                await _publishEndpoint.Publish(eventMessage);
            }
            catch (RequestTimeoutException ex)
            {
                _logger.LogError(ex, "Timeout occurred while sending SaleAddedEvent.");
                throw new ApplicationException("The catalog service did not respond in time.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send SaleAddedEvent.");
                throw new ApplicationException("An error occurred while processing your request.");
            }
        }
    }
}
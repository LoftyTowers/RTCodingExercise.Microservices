using AutoMapper;
using MassTransit;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents;
using RTCodingExercise.Microservices.Models;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;

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

            var eventMessage = _mapper.Map<PlateDto>(plate);
            _logger.LogInformation("Publishing PlateAddedEvent for registration {Registration}", plate.Registration);
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
}

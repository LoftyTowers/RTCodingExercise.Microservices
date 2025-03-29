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
        var eventMessage = _mapper.Map<PlateAddedEvent>(plate);
        _logger.LogInformation("Publishing PlateAddedEvent for registration {Registration}", plate.Registration);
        await _publishEndpoint.Publish(eventMessage);
    }
}

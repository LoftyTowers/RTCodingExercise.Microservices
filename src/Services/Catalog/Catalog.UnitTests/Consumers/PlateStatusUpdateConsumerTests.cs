using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using AutoMapper;
using MassTransit;
using System.Threading.Tasks;
using Catalog.API.Consumers;
using Catalog.API.Services;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;

public class PlateStatusUpdateConsumerTests
{
    private readonly Mock<IPlateService> _mockPlateService;
    private readonly Mock<ILogger<PlateStatusUpdateConsumer>> _mockLogger;
    private readonly Mock<IMapper> _mockMapper;
    private readonly PlateStatusUpdateConsumer _consumer;

    public PlateStatusUpdateConsumerTests()
    {
        _mockPlateService = new Mock<IPlateService>();
        _mockLogger = new Mock<ILogger<PlateStatusUpdateConsumer>>();
        _mockMapper = new Mock<IMapper>();
        _consumer = new PlateStatusUpdateConsumer(_mockPlateService.Object, _mockLogger.Object, _mockMapper.Object);
    }
}


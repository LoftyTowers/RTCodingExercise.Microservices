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

public class SellPlateConsumerTests
{
    private readonly Mock<IPlateService> _mockPlateService;
    private readonly Mock<ILogger<SellPlateConsumer>> _mockLogger;
    private readonly Mock<IMapper> _mockMapper;
    private readonly SellPlateConsumer _consumer;

    public SellPlateConsumerTests()
    {
        _mockPlateService = new Mock<IPlateService>();
        _mockLogger = new Mock<ILogger<SellPlateConsumer>>();
        _mockMapper = new Mock<IMapper>();
        _consumer = new SellPlateConsumer(_mockPlateService.Object, _mockMapper.Object, _mockLogger.Object);
    }
}

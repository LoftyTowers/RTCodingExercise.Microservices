using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using MassTransit;
using System.Threading.Tasks;
using Catalog.API.Consumers;
using Catalog.API.Services;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;

public class PlateSoldConsumerTests
{
    private readonly Mock<ISalesService> _mockSalesService;
    private readonly Mock<ILogger<PlateSoldConsumer>> _mockLogger;
    private readonly PlateSoldConsumer _consumer;

    public PlateSoldConsumerTests()
    {
        _mockSalesService = new Mock<ISalesService>();
        _mockLogger = new Mock<ILogger<PlateSoldConsumer>>();
        _consumer = new PlateSoldConsumer(_mockSalesService.Object, _mockLogger.Object);
    }
}

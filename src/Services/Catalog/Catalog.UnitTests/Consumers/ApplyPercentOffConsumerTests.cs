using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using MassTransit;
using System.Threading.Tasks;
using Catalog.API.Consumers;
using Catalog.API.Services;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;

public class ApplyPercentOffConsumerTests
{
    private readonly Mock<IPromotionService> _mockPromotionService;
    private readonly Mock<ILogger<ApplyPercentOffConsumer>> _mockLogger;
    private readonly ApplyPercentOffConsumer _consumer;

    public ApplyPercentOffConsumerTests()
    {
        _mockPromotionService = new Mock<IPromotionService>();
        _mockLogger = new Mock<ILogger<ApplyPercentOffConsumer>>();
        _consumer = new ApplyPercentOffConsumer(_mockPromotionService.Object, _mockLogger.Object);
    }
}

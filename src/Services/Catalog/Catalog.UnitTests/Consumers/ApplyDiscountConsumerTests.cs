using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using MassTransit;
using System.Threading.Tasks;
using Catalog.API.Consumers;
using Catalog.API.Services;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;

public class ApplyDiscountConsumerTests
{
    private readonly Mock<IPromotionService> _mockPromotionService;
    private readonly Mock<ILogger<ApplyDiscountConsumer>> _mockLogger;
    private readonly ApplyDiscountConsumer _consumer;

    public ApplyDiscountConsumerTests()
    {
        _mockPromotionService = new Mock<IPromotionService>();
        _mockLogger = new Mock<ILogger<ApplyDiscountConsumer>>();
        _consumer = new ApplyDiscountConsumer(_mockPromotionService.Object, _mockLogger.Object);
    }
}

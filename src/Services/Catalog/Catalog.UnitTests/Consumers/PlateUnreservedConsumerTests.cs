using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using MassTransit;
using System.Threading.Tasks;
using Catalog.API.Consumers;
using Catalog.API.Services;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;

public class PlateUnreservedConsumerTests
{
    private readonly Mock<IReservationService> _mockReservationService;
    private readonly Mock<ILogger<PlateUnreservedConsumer>> _mockLogger;
    private readonly PlateUnreservedConsumer _consumer;

    public PlateUnreservedConsumerTests()
    {
        _mockReservationService = new Mock<IReservationService>();
        _mockLogger = new Mock<ILogger<PlateUnreservedConsumer>>();
        _consumer = new PlateUnreservedConsumer(_mockReservationService.Object, _mockLogger.Object);
    }
}

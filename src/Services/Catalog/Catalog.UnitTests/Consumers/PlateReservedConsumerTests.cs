using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using MassTransit;
using System.Threading.Tasks;
using Catalog.API.Consumers;
using Catalog.API.Services;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;

public class PlateReservedConsumerTests
{
    private readonly Mock<IReservationService> _mockReservationService;
    private readonly Mock<ILogger<PlateReservedConsumer>> _mockLogger;
    private readonly PlateReservedConsumer _consumer;

    public PlateReservedConsumerTests()
    {
        _mockReservationService = new Mock<IReservationService>();
        _mockLogger = new Mock<ILogger<PlateReservedConsumer>>();
        _consumer = new PlateReservedConsumer(_mockReservationService.Object, _mockLogger.Object);
    }
}


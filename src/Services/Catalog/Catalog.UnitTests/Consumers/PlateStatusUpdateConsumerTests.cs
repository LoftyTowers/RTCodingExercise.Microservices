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
using System;
using Catalog.UnitTests;

namespace Catalog.API.UnitTests.Consumers
{
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

        [Fact]
        public async Task Consume_Should_Call_UpdateStatusAsync_With_Mapped_Plate()
        {
            // Arrange
            var plateDto = new PlateDto { Id = Guid.NewGuid(), Registration = "XYZ123" };
            var mappedPlate = new Catalog.Domain.Plate { Id = Guid.NewGuid(), Registration = "XYZ123" };

            var eventMessage = new PlateStatusUpdateEvent(plateDto);
            var contextMock = new Mock<ConsumeContext<PlateStatusUpdateEvent>>();
            contextMock.Setup(c => c.Message).Returns(eventMessage);

            _mockMapper.Setup(m => m.Map<Catalog.Domain.Plate>(plateDto)).Returns(mappedPlate);

            // Act
            await _consumer.Consume(contextMock.Object);

            // Assert
            _mockPlateService.Verify(s => s.UpdateStatusAsync(mappedPlate), Times.Once);
        }
    }
}
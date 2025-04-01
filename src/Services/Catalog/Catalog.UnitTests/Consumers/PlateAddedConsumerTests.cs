using System;
using System.Threading.Tasks;
using Catalog.API.Consumers;
using Catalog.API.Services;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;
using Xunit;

namespace Catalog.API.UnitTests.Consumers
{
    public class PlateAddedConsumerTests
    {
        private readonly Mock<IPlateService> _plateServiceMock;
        private readonly Mock<ILogger<PlateAddedConsumer>> _loggerMock;
        private readonly PlateAddedConsumer _consumer;

        public PlateAddedConsumerTests()
        {
            _plateServiceMock = new Mock<IPlateService>();
            _loggerMock = new Mock<ILogger<PlateAddedConsumer>>();
            _consumer = new PlateAddedConsumer(_loggerMock.Object, _plateServiceMock.Object);
        }

        // Test: Consume_Should_Call_AddPlateAsync_With_Correct_Plate
        // - Verify that IPlateService.AddPlateAsync is called once with context.Message.Plate
        [Fact]
        public async Task Consume_Should_Call_AddPlateAsync_With_Correct_Plate()
        {
            // Arrange
            var plateDto = new PlateDto { Registration = "XYZ789" };
            var eventMessage = new PlateAddedEvent { Plate = plateDto };
            var contextMock = new Mock<ConsumeContext<PlateAddedEvent>>();
            contextMock.Setup(x => x.Message).Returns(eventMessage);

            // Act
            await _consumer.Consume(contextMock.Object);

            // Assert: Verify the service method was called with the correct DTO.
            _plateServiceMock.Verify(s => s.AddPlateAsync(plateDto), Times.Once);
        }

        // Test: Consume_Should_Rethrow_Exception_When_AddPlateAsync_Fails
        // - Simulate AddPlateAsync throwing an exception and assert that Consume rethrows it
        [Fact]
        public async Task Consume_Should_Rethrow_Exception_When_AddPlateAsync_Fails()
        {
            // Arrange
            var plateDto = new PlateDto { Registration = "XYZ789" };
            var eventMessage = new PlateAddedEvent { Plate = plateDto };
            var contextMock = new Mock<ConsumeContext<PlateAddedEvent>>();
            contextMock.Setup(x => x.Message).Returns(eventMessage);
            _plateServiceMock.Setup(s => s.AddPlateAsync(It.IsAny<PlateDto>())).ThrowsAsync(new Exception("Test error"));

            // Act & Assert: Verify that the exception is rethrown
            await Assert.ThrowsAsync<Exception>(() => _consumer.Consume(contextMock.Object));
        }

        // Test: Consume_Should_Handle_Valid_Message_Without_Errors
        // - End-to-end test to verify no exceptions are thrown when a valid PlateAddedEvent is consumed
        [Fact]
        public async Task Consume_Should_Handle_Valid_Message_Without_Errors()
        {
            // Arrange
            var plateDto = new PlateDto { Registration = "XYZ789" };
            var eventMessage = new PlateAddedEvent { Plate = plateDto };
            var contextMock = new Mock<ConsumeContext<PlateAddedEvent>>();
            contextMock.Setup(x => x.Message).Returns(eventMessage);

            // Act: Call the Consume method
            await _consumer.Consume(contextMock.Object);

            // Assert: No exceptions should be thrown, and the method should complete successfully
        }

        // Test: Consume_Should_Throw_When_Context_Message_Is_Null
        // - Simulate context.Message as null and verify that a NullReferenceException or similar is thrown
        [Fact]
        public async Task Consume_Should_Throw_When_Context_Message_Is_Null()
        {
            // Arrange
            var contextMock = new Mock<ConsumeContext<PlateAddedEvent>>();
            contextMock.Setup(x => x.Message).Returns((PlateAddedEvent)null);

            // Act & Assert: Verify that a NullReferenceException is thrown
            await Assert.ThrowsAsync<NullReferenceException>(() => _consumer.Consume(contextMock.Object));
        }

        // Test: Consume_Should_Throw_When_Plate_Is_Null
        // - Simulate context.Message.Plate as null and verify the method throws
        [Fact]
        public async Task Consume_Should_Throw_When_Plate_Is_Null()
        {
            // Arrange
            var eventMessage = new PlateAddedEvent { Plate = null };
            var contextMock = new Mock<ConsumeContext<PlateAddedEvent>>();
            contextMock.Setup(x => x.Message).Returns(eventMessage);

            // Act & Assert: Verify that a NullReferenceException is thrown
            await Assert.ThrowsAsync<NullReferenceException>(() => _consumer.Consume(contextMock.Object));
        }
    }
}

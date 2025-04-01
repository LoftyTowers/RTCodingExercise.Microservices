using Xunit;
using Moq;
using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using RTCodingExercise.Microservices.WebMVC.Services;
using RTCodingExercise.Microservices.Models;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;
using System;
using System.Threading.Tasks;

namespace WebMVC.UnitTests.Services
{
    public class PlateCommandServiceTests
    {
        private readonly Mock<IPublishEndpoint> _mockPublisher;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<PlateCommandService>> _mockLogger;
        private readonly PlateCommandService _service;


        public PlateCommandServiceTests()
        {
            _mockPublisher = new Mock<IPublishEndpoint>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<PlateCommandService>>();
            _service = new PlateCommandService(_mockPublisher.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task AddPlateAsync_Should_Map_And_PublishEvent_When_ValidPlateProvided()
        {
            // Arrange
            var plate = new PlateViewModel
            {
                Id = Guid.NewGuid(),
                Registration = "ABC123",
                PurchasePrice = 1000,
                SalePrice = 1500
            };

            var plateDto = new PlateDto
            {
                Id = plate.Id,
                Registration = plate.Registration,
                PurchasePrice = plate.PurchasePrice,
                SalePrice = plate.SalePrice
            };

            _mockMapper.Setup(m => m.Map<PlateDto>(plate)).Returns(plateDto);

            // Act
            await _service.AddPlateAsync(plate);

            // Assert
            _mockPublisher.Verify(p => p.Publish(It.Is<PlateAddedEvent>(e =>
                e.Plate.Registration == plateDto.Registration &&
                e.Plate.PurchasePrice == plateDto.PurchasePrice &&
                e.Plate.SalePrice == plateDto.SalePrice
            ), default), Times.Once);
        }

        [Fact]
        public async Task AddPlateAsync_Should_ThrowApplicationException_When_TimeoutOccurs()
        {
            // Arrange
            var plate = new PlateViewModel { Registration = "XYZ123" };
            var plateDto = new PlateDto { Registration = "XYZ123" };

            _mockMapper.Setup(m => m.Map<PlateDto>(It.IsAny<PlateViewModel>())).Returns(plateDto);
            _mockPublisher
                .Setup(p => p.Publish(It.IsAny<PlateAddedEvent>(), default))
                .ThrowsAsync(new RequestTimeoutException("Timeout"));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => _service.AddPlateAsync(plate));
            Assert.Contains("did not respond in time", ex.Message);
        }

        [Fact]
        public async Task AddPlateAsync_Should_ThrowApplicationException_When_PublishFails()
        {
            // Arrange
            var plate = new PlateViewModel { Registration = "XYZ123" };
            var plateDto = new PlateDto { Registration = "XYZ123" };

            _mockMapper.Setup(m => m.Map<PlateDto>(It.IsAny<PlateViewModel>())).Returns(plateDto);
            _mockPublisher
                .Setup(p => p.Publish(It.IsAny<PlateAddedEvent>(), default))
                .ThrowsAsync(new Exception("Unhandled error"));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => _service.AddPlateAsync(plate));
            Assert.Contains("An error occurred while processing your request", ex.Message);
        }

        [Fact]
        public async Task ToggleReservationAsync_Should_ToggleAndPublishEvent_When_ValidPlateProvided()
        {
            // Arrange
            var originalIsReserved = false;
            var plate = new PlateViewModel
            {
                Id = Guid.NewGuid(),
                Registration = "TEST123",
                IsReserved = originalIsReserved
            };

            var expectedDto = new PlateDto
            {
                Id = plate.Id,
                Registration = plate.Registration,
                IsReserved = !originalIsReserved // should be flipped
            };

            _mockMapper.Setup(m => m.Map<PlateDto>(It.IsAny<PlateViewModel>()))
                       .Returns(expectedDto);

            // Act
            await _service.ToggleReservationAsync(plate);

            // Assert
            _mockPublisher.Verify(p => p.Publish(It.Is<PlateReserveToggleEvent>(e =>
                e.Plate.Id == expectedDto.Id &&
                e.Plate.IsReserved == expectedDto.IsReserved
            ), default), Times.Once);
        }

        [Fact]
        public async Task ToggleReservationAsync_Should_ThrowApplicationException_When_TimeoutOccurs()
        {
            // Arrange
            var plate = new PlateViewModel { Id = Guid.NewGuid(), Registration = "TIMEOUT", IsReserved = false };
            var dto = new PlateDto { Id = plate.Id, Registration = "TIMEOUT", IsReserved = true };

            _mockMapper.Setup(m => m.Map<PlateDto>(plate)).Returns(dto);
            _mockPublisher
                .Setup(p => p.Publish(It.IsAny<PlateReserveToggleEvent>(), default))
                .ThrowsAsync(new RequestTimeoutException("Timeout"));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => _service.ToggleReservationAsync(plate));
            Assert.Contains("did not respond in time", ex.Message);
        }

        [Fact]
        public async Task ToggleReservationAsync_Should_ThrowApplicationException_When_PublishFails()
        {
            // Arrange
            var plate = new PlateViewModel { Id = Guid.NewGuid(), Registration = "FAIL", IsReserved = false };
            var dto = new PlateDto { Id = plate.Id, Registration = "FAIL", IsReserved = true };

            _mockMapper.Setup(m => m.Map<PlateDto>(plate)).Returns(dto);
            _mockPublisher
                .Setup(p => p.Publish(It.IsAny<PlateReserveToggleEvent>(), default))
                .ThrowsAsync(new Exception("Unhandled"));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => _service.ToggleReservationAsync(plate));
            Assert.Contains("An error occurred while processing your request", ex.Message);
        }
    }
}
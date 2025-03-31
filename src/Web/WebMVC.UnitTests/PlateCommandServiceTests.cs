using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;
using RTCodingExercise.Microservices.Models;

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
    public async Task AddPlateAsync_Should_ThrowArgumentNullException_When_PlateIsNull()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _service.AddPlateAsync(null));
    }

    [Fact]
    public async Task AddPlateAsync_Should_ThrowArgumentException_When_RegistrationIsNullOrWhitespace()
    {
        // Arrange
        var plate = new PlateViewModel { Registration = " " };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.AddPlateAsync(plate));
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
}

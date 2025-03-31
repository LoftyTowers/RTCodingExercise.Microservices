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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class PlateQueryServiceTests
{
    private readonly Mock<IRequestClient<GetPlatesEvent>> _mockClient;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<PlateQueryService>> _mockLogger;
    private readonly PlateQueryService _service;

    public PlateQueryServiceTests()
    {
        _mockClient = new Mock<IRequestClient<GetPlatesEvent>>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<PlateQueryService>>();

        _service = new PlateQueryService(_mockClient.Object, _mockMapper.Object, _mockLogger.Object);
    }

    // Test: GetPlatesAsync_Should_ReturnMappedPlates_When_ResponseContainsPlates
    // - Simulate PlatesRetrievedEvent with valid Plates
    [Fact]
    public async Task GetPlatesAsync_ReturnsMappedPlates_WhenResponseContainsPlates()
    {
        // Arrange
        var plateDtos = new List<PlateDto>
        {
            new PlateDto { Id = Guid.NewGuid(), Registration = "ABC123", PurchasePrice = 1000, SalePrice = 1500 }
        };

        var expectedViewModels = new List<PlateViewModel>
        {
            new PlateViewModel { Id = plateDtos[0].Id, Registration = "ABC123", PurchasePrice = 1000, SalePrice = 1500 }
        };

        var responseMock = new Mock<Response<PlatesRetrievedEvent>>();
        responseMock.Setup(r => r.Message).Returns(new PlatesRetrievedEvent(plateDtos));


        _mockClient
            .Setup(c => c.GetResponse<PlatesRetrievedEvent>(It.IsAny<GetPlatesEvent>(), default, default))
            .ReturnsAsync(responseMock.Object);

        _mockMapper
            .Setup(m => m.Map<List<PlateViewModel>>(plateDtos))
            .Returns(expectedViewModels);

        // Act
        var result = await _service.GetPlatesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("ABC123", result[0].Registration);
    }

    // Test: GetPlatesAsync_Should_Return_EmptyList_When_ResponseHasNullPlates
    // - Simulate PlatesRetrievedEvent.Plates = null
    [Fact]
    public async Task GetPlatesAsync_Should_Return_EmptyList_When_ResponseHasNullPlates()
    {
        // Arrange
        var responseMock = new Mock<Response<PlatesRetrievedEvent>>();
        responseMock.Setup(r => r.Message).Returns(new PlatesRetrievedEvent(null));

        _mockClient
            .Setup(c => c.GetResponse<PlatesRetrievedEvent>(It.IsAny<GetPlatesEvent>(), default, default))
            .ReturnsAsync(responseMock.Object);

        // Act
        var result = await _service.GetPlatesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}

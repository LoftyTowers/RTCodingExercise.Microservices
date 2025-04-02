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
using System.Threading.Tasks;
using System.Threading;
using WebMVC.Enums;
using System.Linq;

namespace WebMVC.UnitTests.Services
{
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

        [Fact]
        public async Task GetSortedPlatesAsync_ReturnsMappedPlates_WhenResponseContainsPlates()
        {
            // Arrange
            var plateDataDtos = new PlateDataDto
            {
                Plates = new List<PlateDto>
                {
                    new PlateDto { Id = Guid.NewGuid(), Registration = "ABC123", PurchasePrice = 1000, SalePrice = 1500 }
                },
                AverageProfitMargin = 500,
                TotalRevenue = 1500
            };

            var expectedViewModels = new PlateDataViewModel
            {
                Plates = new List<PlateViewModel>
                {
                    new PlateViewModel { Id = plateDataDtos.Plates[0].Id, Registration = "ABC123", PurchasePrice = 1000, SalePrice = 1500 }
                },
                AverageProfitMargin = 500,
                TotalRevenue = 1500
            };

            var responseMock = new Mock<Response<PlatesRetrievedEvent>>();
            responseMock.Setup(r => r.Message).Returns(new PlatesRetrievedEvent(plateDataDtos));

            _mockClient
                .Setup(c => c.GetResponse<PlatesRetrievedEvent>(
                    It.IsAny<GetPlatesEvent>(),
                    It.IsAny<CancellationToken>(),
                    default))
                .ReturnsAsync(responseMock.Object);

            _mockMapper
                .Setup(m => m.Map<PlateDataViewModel>(plateDataDtos))
                .Returns(expectedViewModels);

            // Act
            var result = await _service.GetSortedPlatesAsync(SortField.Registration, SortDirection.Ascending);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Plates);
            Assert.Single(result.Plates);
            Assert.Equal("ABC123", result.Plates.ToList().FirstOrDefault()?.Registration);
        }

        [Fact]
        public async Task GetSortedPlatesAsync_ReturnsEmptyList_WhenResponseIsNull()
        {
            // Arrange
            var responseMock = new Mock<Response<PlatesRetrievedEvent>>();
            responseMock.Setup(r => r.Message).Returns(new PlatesRetrievedEvent(null));

            _mockClient
                .Setup(c => c.GetResponse<PlatesRetrievedEvent>(
                    It.IsAny<GetPlatesEvent>(),
                    It.IsAny<CancellationToken>(),
                    default))
                .ReturnsAsync(responseMock.Object);

            // Act
            var result = await _service.GetSortedPlatesAsync(SortField.Registration, SortDirection.Ascending);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task FilterPlatesAsync_ReturnsMappedResults()
        {
            // Arrange
            var plateDataDtos = new PlateDataDto
            {
                Plates = new List<PlateDto>
                {
                    new PlateDto { Id = Guid.NewGuid(), Registration = "D44NNY", PurchasePrice = 1000, SalePrice = 2000 }
                },
                AverageProfitMargin = 1000,
                TotalRevenue = 2000
            };

            var expectedViewModels = new PlateDataViewModel
            {
                Plates = new List<PlateViewModel>
                {
                    new PlateViewModel { Id = plateDataDtos.Plates[0].Id, Registration = "D44NNY", PurchasePrice = 1000, SalePrice = 2000 }
                },
                AverageProfitMargin = 1000,
                TotalRevenue = 2000
            };

            var responseMock = new Mock<Response<PlatesRetrievedEvent>>();
            responseMock.Setup(r => r.Message).Returns(new PlatesRetrievedEvent(plateDataDtos));

            _mockClient
                .Setup(c => c.GetResponse<PlatesRetrievedEvent>(
                    It.IsAny<GetPlatesEvent>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<RequestTimeout>()))
                .ReturnsAsync(responseMock.Object);

            _mockMapper
                .Setup(m => m.Map<PlateDataViewModel>(plateDataDtos))
                .Returns(expectedViewModels);

            // Act
            var result = await _service.FilterPlatesAsync("Danny", false);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Plates);
            Assert.Single(result.Plates);
            Assert.Equal("D44NNY", result.Plates.ToList().FirstOrDefault()?.Registration);
        }
    }
}
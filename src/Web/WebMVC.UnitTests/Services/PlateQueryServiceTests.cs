
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
        private readonly Mock<IRequestClient<GetPlatesEvent>> _mockGetPlatesClient;
        private readonly Mock<IRequestClient<PlateAddedEvent>> _mockAddPlateClient;
        private readonly Mock<IRequestClient<PlateStatusUpdateEvent>> _mockUpdateStatusClient;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<PlateQueryService>> _mockLogger;
        private readonly PlateQueryService _service;

        public PlateQueryServiceTests()
        {
            _mockGetPlatesClient = new Mock<IRequestClient<GetPlatesEvent>>();
            _mockAddPlateClient = new Mock<IRequestClient<PlateAddedEvent>>();
            _mockUpdateStatusClient = new Mock<IRequestClient<PlateStatusUpdateEvent>>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<PlateQueryService>>();

            _service = new PlateQueryService(
                _mockGetPlatesClient.Object,
                _mockAddPlateClient.Object,
                _mockUpdateStatusClient.Object,
                _mockMapper.Object,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task GetSortedPlatesAsync_ReturnsMappedPlates_WhenResponseContainsPlates()
        {
            var plateDataDtos = new PlateDataDto
            {
                Plates = new List<PlateDto> { new PlateDto { Id = Guid.NewGuid(), Registration = "ABC123", PurchasePrice = 1000, SalePrice = 1500 } },
                AverageProfitMargin = 500,
                TotalRevenue = 1500
            };

            var expectedViewModels = new PlateDataViewModel
            {
                Plates = new List<PlateViewModel> { new PlateViewModel { Id = plateDataDtos.Plates[0].Id, Registration = "ABC123", PurchasePrice = 1000, SalePrice = 1500 } },
                AverageProfitMargin = 500,
                TotalRevenue = 1500
            };

            var responseMock = new Mock<Response<PlatesRetrievedEvent>>();
            responseMock.Setup(r => r.Message).Returns(new PlatesRetrievedEvent(plateDataDtos));

            _mockGetPlatesClient
                .Setup(c => c.GetResponse<PlatesRetrievedEvent>(It.IsAny<GetPlatesEvent>(), It.IsAny<CancellationToken>(), default))
                .ReturnsAsync(responseMock.Object);

            _mockMapper.Setup(m => m.Map<PlateDataViewModel>(plateDataDtos)).Returns(expectedViewModels);

            var result = await _service.GetSortedPlatesAsync(SortField.Registration, SortDirection.Ascending);

            Assert.NotNull(result);
            Assert.NotNull(result.Plates);
            Assert.Single(result.Plates);
            Assert.Equal("ABC123", result.Plates.First().Registration);
        }

        [Fact]
        public async Task GetSortedPlatesAsync_ReturnsEmptyList_WhenResponseIsNull()
        {
            var responseMock = new Mock<Response<PlatesRetrievedEvent>>();
            responseMock.Setup(r => r.Message).Returns(new PlatesRetrievedEvent(null));

            _mockGetPlatesClient
                .Setup(c => c.GetResponse<PlatesRetrievedEvent>(It.IsAny<GetPlatesEvent>(), It.IsAny<CancellationToken>(), default))
                .ReturnsAsync(responseMock.Object);

            var result = await _service.GetSortedPlatesAsync(SortField.Registration, SortDirection.Ascending);

            Assert.NotNull(result);
            Assert.Empty(result.Plates);
        }

        [Fact]
        public async Task FilterPlatesAsync_ReturnsMappedResults()
        {
            var plateDataDtos = new PlateDataDto
            {
                Plates = new List<PlateDto> { new PlateDto { Id = Guid.NewGuid(), Registration = "D44NNY", PurchasePrice = 1000, SalePrice = 2000 } },
                AverageProfitMargin = 1000,
                TotalRevenue = 2000
            };

            var expectedViewModels = new PlateDataViewModel
            {
                Plates = new List<PlateViewModel> { new PlateViewModel { Id = plateDataDtos.Plates[0].Id, Registration = "D44NNY", PurchasePrice = 1000, SalePrice = 2000 } },
                AverageProfitMargin = 1000,
                TotalRevenue = 2000
            };

            var responseMock = new Mock<Response<PlatesRetrievedEvent>>();
            responseMock.Setup(r => r.Message).Returns(new PlatesRetrievedEvent(plateDataDtos));

            _mockGetPlatesClient
                .Setup(c => c.GetResponse<PlatesRetrievedEvent>(It.IsAny<GetPlatesEvent>(), It.IsAny<CancellationToken>(), It.IsAny<RequestTimeout>()))
                .ReturnsAsync(responseMock.Object);

            _mockMapper.Setup(m => m.Map<PlateDataViewModel>(plateDataDtos)).Returns(expectedViewModels);

            var result = await _service.FilterPlatesAsync("Danny", false);

            Assert.NotNull(result);
            Assert.NotNull(result.Plates);
            Assert.Single(result.Plates);
            Assert.Equal("D44NNY", result.Plates.First().Registration);
        }

        [Fact]
        public async Task AddPlateAsync_Should_Map_And_PublishEvent_When_ValidPlateProvided()
        {
            var plate = new PlateViewModel { Id = Guid.NewGuid(), Registration = "ABC123", PurchasePrice = 1000, SalePrice = 1500 };
            var dto = new PlateDto { Id = plate.Id, Registration = "ABC123", PurchasePrice = 1000, SalePrice = 1500 };
            var plateDataDto = new PlateDataDto { Plates = new List<PlateDto> { dto } };

            var responseMock = new Mock<Response<PlateAddedCompletedEvent>>();
            responseMock.Setup(r => r.Message).Returns(new PlateAddedCompletedEvent(plateDataDto));

            _mockMapper.Setup(m => m.Map<PlateDto>(plate)).Returns(dto);
            _mockMapper.Setup(m => m.Map<PlateDataViewModel>(plateDataDto))
                       .Returns(new PlateDataViewModel { Plates = new List<PlateViewModel> { new PlateViewModel { Registration = "ABC123" } } });

            _mockAddPlateClient
                .Setup(p => p.GetResponse<PlateAddedCompletedEvent>(It.IsAny<PlateAddedEvent>(), It.IsAny<CancellationToken>(), default))
                .ReturnsAsync(responseMock.Object);

            var result = await _service.AddPlateAsync(plate);

            Assert.NotNull(result);
            Assert.Single(result.Plates);
            Assert.Equal("ABC123", result.Plates.First().Registration);
        }

        [Fact]
        public async Task AddPlateAsync_Should_ThrowApplicationException_When_TimeoutOccurs()
        {
            var plate = new PlateViewModel { Registration = "XYZ123" };
            var dto = new PlateDto { Registration = "XYZ123" };

            _mockMapper.Setup(m => m.Map<PlateDto>(plate)).Returns(dto);
            _mockAddPlateClient
                .Setup(p => p.GetResponse<PlateAddedCompletedEvent>(It.IsAny<PlateAddedEvent>(), It.IsAny<CancellationToken>(), default))
                .ThrowsAsync(new RequestTimeoutException("Timeout"));

            var ex = await Assert.ThrowsAsync<ApplicationException>(() => _service.AddPlateAsync(plate));
            Assert.Contains("did not respond in time", ex.Message);
        }

        [Fact]
        public async Task UpdateStatusAsync_Should_ToggleAndPublishEvent_When_ValidPlateProvided()
        {
            var plate = new PlateViewModel { Id = Guid.NewGuid(), Registration = "TEST123", Status = Status.Reserved };
            var dto = new PlateDto { Id = plate.Id, Registration = plate.Registration, Status = EventBus.Enums.Status.Reserved };
            var plateDataDto = new PlateDataDto { Plates = new List<PlateDto> { dto } };

            var responseMock = new Mock<Response<PlateStatusUpdateCompletedEvent>>();
            responseMock.Setup(r => r.Message).Returns(new PlateStatusUpdateCompletedEvent(plateDataDto));

            _mockMapper.Setup(m => m.Map<PlateDto>(plate)).Returns(dto);
            _mockMapper.Setup(m => m.Map<PlateDataViewModel>(plateDataDto))
                       .Returns(new PlateDataViewModel { Plates = new List<PlateViewModel> { new PlateViewModel { Registration = "TEST123" } } });

            _mockUpdateStatusClient
                .Setup(p => p.GetResponse<PlateStatusUpdateCompletedEvent>(It.IsAny<PlateStatusUpdateEvent>(), It.IsAny<CancellationToken>(), default))
                .ReturnsAsync(responseMock.Object);

            var result = await _service.UpdateStatusAsync(plate);

            Assert.NotNull(result);
            Assert.Single(result.Plates);
        }

        [Fact]
        public async Task UpdateStatusAsync_Should_ThrowApplicationException_When_TimeoutOccurs()
        {
            var plate = new PlateViewModel { Id = Guid.NewGuid(), Registration = "TIMEOUT", Status = Status.Reserved };
            var dto = new PlateDto { Id = plate.Id, Registration = "TIMEOUT", Status = EventBus.Enums.Status.Reserved };

            _mockMapper.Setup(m => m.Map<PlateDto>(plate)).Returns(dto);
            _mockUpdateStatusClient
                .Setup(p => p.GetResponse<PlateStatusUpdateCompletedEvent>(It.IsAny<PlateStatusUpdateEvent>(), It.IsAny<CancellationToken>(), default))
                .ThrowsAsync(new RequestTimeoutException("Timeout"));

            var ex = await Assert.ThrowsAsync<ApplicationException>(() => _service.UpdateStatusAsync(plate));
            Assert.Contains("did not respond in time", ex.Message);
        }

        [Fact]
        public async Task UpdateStatusAsync_Should_ThrowApplicationException_When_PublishFails()
        {
            var plate = new PlateViewModel { Id = Guid.NewGuid(), Registration = "FAIL", Status = Status.Reserved };
            var dto = new PlateDto { Id = plate.Id, Registration = "FAIL", Status = EventBus.Enums.Status.Reserved };

            _mockMapper.Setup(m => m.Map<PlateDto>(plate)).Returns(dto);
            _mockUpdateStatusClient
                .Setup(p => p.GetResponse<PlateStatusUpdateCompletedEvent>(It.IsAny<PlateStatusUpdateEvent>(), It.IsAny<CancellationToken>(), default))
                .ThrowsAsync(new Exception("Unhandled"));

            var ex = await Assert.ThrowsAsync<ApplicationException>(() => _service.UpdateStatusAsync(plate));
            Assert.Contains("An error occurred while processing your request", ex.Message);
        }
    }
}

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
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace WebMVC.UnitTests.Services
{
    public class SalesQueryServiceTests
    {
        private readonly Mock<IRequestClient<SellPlateEvent>> _mockClient;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<SalesQueryService>> _mockLogger;
        private readonly SalesQueryService _service;


        public SalesQueryServiceTests()
        {
            _mockClient = new Mock<IRequestClient<SellPlateEvent>>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<SalesQueryService>>();
            _service = new SalesQueryService(_mockClient.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task SellPlate_Should_Map_And_Send_SellPlateEvent_And_Return_PlateData()
        {
            // Arrange
            var plate = new PlateViewModel
            {
                Id = Guid.NewGuid(),
                FinalSalePrice = 999.99M,
                PromoCodeUsed = "SUMMER23"
            };

            var dto = new PlateDto
            {
                Id = plate.Id,
                FinalSalePrice = plate.FinalSalePrice,
                PromoCodeUsed = plate.PromoCodeUsed
            };

            var plateDataDto = new PlateDataDto
            {
                Plates = new List<PlateDto> { dto }
            };

            var viewModel = new PlateDataViewModel
            {
                Plates = new List<PlateViewModel> { plate }
            };

            var completedEvent = new SellPlateCompletedEvent(plateDataDto);

            var responseMock = new Mock<Response<SellPlateCompletedEvent>>();
            responseMock.Setup(r => r.Message).Returns(completedEvent);

            _mockMapper.Setup(m => m.Map<PlateDto>(plate)).Returns(dto);
            _mockMapper.Setup(m => m.Map<PlateDataViewModel>(plateDataDto)).Returns(viewModel);

            _mockClient
                .Setup(c => c.GetResponse<SellPlateCompletedEvent>(
                    It.IsAny<SellPlateEvent>(),
                    It.IsAny<CancellationToken>(),
                    default))
                .ReturnsAsync(responseMock.Object);

            // Act
            var result = await _service.SellPlate(plate);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Plates);
            Assert.Equal(plate.Id, result.Plates.First().Id);
        }

    }
}
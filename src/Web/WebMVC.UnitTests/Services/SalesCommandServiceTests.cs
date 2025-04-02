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
    public class SalesCommandServiceTests
    {
        private readonly Mock<IPublishEndpoint> _mockPublisher;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<SalesCommandService>> _mockLogger;
        private readonly SalesCommandService _service;


        public SalesCommandServiceTests()
        {
            _mockPublisher = new Mock<IPublishEndpoint>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<SalesCommandService>>();
            _service = new SalesCommandService(_mockPublisher.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task SellPlate_Should_Map_And_Publish_SellPlateEvent()
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

            _mockMapper.Setup(m => m.Map<PlateDto>(plate)).Returns(dto);

            // Act
            await _service.SellPlate(plate);

            // Assert
            _mockPublisher.Verify(p => p.Publish(It.Is<SellPlateEvent>(e =>
                e.Plate.Id == dto.Id &&
                e.Plate.FinalSalePrice == dto.FinalSalePrice &&
                e.Plate.PromoCodeUsed == dto.PromoCodeUsed &&
                e.CorrelationId != Guid.Empty
            ), default), Times.Once);
        }
    }
}
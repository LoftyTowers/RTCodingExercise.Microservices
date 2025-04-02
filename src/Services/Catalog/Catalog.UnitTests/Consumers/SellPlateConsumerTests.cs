using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using AutoMapper;
using MassTransit;
using System.Threading.Tasks;
using Catalog.API.Consumers;
using Catalog.API.Services;
using Catalog.Domain;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;
using Catalog.UnitTests;
using System;

namespace Catalog.API.UnitTests.Consumers
{
    public class SellPlateConsumerTests
    {
        private readonly Mock<IPlateService> _mockPlateService;
        private readonly Mock<ILogger<SellPlateConsumer>> _mockLogger;
        private readonly Mock<IMapper> _mockMapper;
        private readonly SellPlateConsumer _consumer;

        public SellPlateConsumerTests()
        {
            _mockPlateService = new Mock<IPlateService>();
            _mockLogger = new Mock<ILogger<SellPlateConsumer>>();
            _mockMapper = new Mock<IMapper>();
            _consumer = new SellPlateConsumer(_mockPlateService.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Consume_Should_Call_SellPlateAsync_With_Mapped_Plate()
        {
            // Arrange
            var dto = new PlateDto { Id = Guid.NewGuid(), Registration = "ZZZ123" };
            var domainPlate = new Plate { Id = Guid.NewGuid(), Registration = "ZZZ123" };

            var eventMessage = new SellPlateEvent( dto, Guid.NewGuid() );
            var contextMock = new Mock<ConsumeContext<SellPlateEvent>>();
            contextMock.Setup(c => c.Message).Returns(eventMessage);

            _mockMapper.Setup(m => m.Map<Plate>(dto)).Returns(domainPlate);

            // Act
            await _consumer.Consume(contextMock.Object);

            // Assert
            _mockPlateService.Verify(s => s.SellPlateAsync(domainPlate), Times.Once);
        }
    }
}
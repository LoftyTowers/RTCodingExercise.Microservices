using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.API.Consumers;
using Catalog.API.Services;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;
using Catalog.Domain;
using Catalog.Domain.Enums;
using Xunit;
using System.Threading;

namespace Catalog.API.UnitTests.Consumers
{
    public class GetPlatesConsumerTests
    {
        private readonly Mock<IPlateService> _plateServiceMock;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<GetPlatesConsumer>> _loggerMock;
        private readonly GetPlatesConsumer _consumer;

        public GetPlatesConsumerTests()
        {
            _plateServiceMock = new Mock<IPlateService>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Plate, PlateDto>()
                    .ForMember(dest => dest.Registration, opt => opt.MapFrom(src => src.Registration));
            });
            _mapper = config.CreateMapper();

            _loggerMock = new Mock<ILogger<GetPlatesConsumer>>();
            _consumer = new GetPlatesConsumer(_loggerMock.Object, _plateServiceMock.Object, _mapper);
        }

        [Fact]
        public async Task Consume_Should_Return_Plates()
        {
            // Arrange
            var request = new GetPlatesEvent
            {
                SortField = EventBus.Enums.SortField.Registration,
                SortDirection = EventBus.Enums.SortDirection.Ascending,
                Filter = "TEST",
                OnlyAvailable = true
            };

            var contextMock = new Mock<ConsumeContext<GetPlatesEvent>>();
            contextMock.Setup(c => c.Message).Returns(request);
            contextMock.Setup(c => c.CorrelationId).Returns(Guid.NewGuid());

            _plateServiceMock.Setup(p => p.GetPlatesAsync(It.IsAny<SortField>(), It.IsAny<SortDirection>(), It.IsAny<string>(), It.IsAny<bool?>()))
                .ReturnsAsync(new PlateDataDto { Plates = new List<PlateDto>() { new PlateDto() { Registration = "ABC123" } } } );

            // Act
            await _consumer.Consume(contextMock.Object);

            // Assert
            contextMock.Verify(c => c.RespondAsync(It.Is<PlatesRetrievedEvent>(r =>
                r.PlateData.Plates.Count == 1 && r.PlateData.Plates.First().Registration == "ABC123")), Times.Once);
        }

        [Fact]
        public async Task Consume_Should_Respond_With_Fault_On_Exception()
        {
            // Arrange
            var contextMock = new Mock<ConsumeContext<GetPlatesEvent>>();
            contextMock.Setup(c => c.Message).Returns(new GetPlatesEvent());
            contextMock.Setup(c => c.CorrelationId).Returns(Guid.NewGuid());

            _plateServiceMock.Setup(p => p.GetPlatesAsync(It.IsAny<SortField>(), It.IsAny<SortDirection>(), It.IsAny<string>(), It.IsAny<bool?>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            await _consumer.Consume(contextMock.Object);

            // Assert
            contextMock.Verify(c => c.RespondAsync<Fault<GetPlatesEvent>>(It.IsAny<Exception>()), Times.Once);
        }

        [Fact]
        public async Task Consume_Should_Use_Default_Guid_When_CorrelationId_Null()
        {
            // Arrange
            var contextMock = new Mock<ConsumeContext<GetPlatesEvent>>();
            contextMock.Setup(c => c.Message).Returns(new GetPlatesEvent());
            contextMock.Setup(c => c.CorrelationId).Returns((Guid?)null);

            _plateServiceMock.Setup(p => p.GetPlatesAsync(It.IsAny<SortField>(), It.IsAny<SortDirection>(), It.IsAny<string>(), It.IsAny<bool?>()))
                .ReturnsAsync(new PlateDataDto());

            PlatesRetrievedEvent capturedResponse = null;
            contextMock.Setup(c => c.RespondAsync(It.IsAny<PlatesRetrievedEvent>()))
                .Callback<object>(resp => capturedResponse = resp as PlatesRetrievedEvent)
                .Returns(Task.CompletedTask);

            // Act
            await _consumer.Consume(contextMock.Object);

            // Assert
            Assert.NotNull(capturedResponse);
            Assert.NotEqual(Guid.Empty, capturedResponse.CorrelationId); // should be new GUID
        }

        [Fact]
        public async Task Consume_Should_Call_GetPlatesAsync_With_Correct_Parameters()
        {
            // Arrange
            var request = new GetPlatesEvent
            {
                SortField = EventBus.Enums.SortField.Registration,
                SortDirection = EventBus.Enums.SortDirection.Descending,
                Filter = "DEF",
                OnlyAvailable = false
            };

            var contextMock = new Mock<ConsumeContext<GetPlatesEvent>>();
            contextMock.Setup(c => c.Message).Returns(request);
            contextMock.Setup(c => c.CorrelationId).Returns(Guid.NewGuid());

            _plateServiceMock.Setup(p => p.GetPlatesAsync(It.IsAny<SortField>(), It.IsAny<SortDirection>(), It.IsAny<string>(), It.IsAny<bool?>()))
                .ReturnsAsync(new PlateDataDto());

            // Act
            await _consumer.Consume(contextMock.Object);

            // Assert
            _plateServiceMock.Verify(p => p.GetPlatesAsync(
                SortField.Registration,
                SortDirection.Descending,
                "DEF",
                false
            ), Times.Once);
        }
    }
}
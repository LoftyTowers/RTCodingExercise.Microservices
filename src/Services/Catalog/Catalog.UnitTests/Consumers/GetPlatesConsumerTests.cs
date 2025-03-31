using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.API.Consumers;
using Catalog.API.Repositories;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;
using Catalog.Domain;
using Xunit;

namespace Catalog.API.UnitTests.Consumers
{
    public class GetPlatesConsumerTests
    {
        private readonly Mock<IPlateRepository> _plateRepositoryMock;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<GetPlatesConsumer>> _loggerMock;
        private readonly GetPlatesConsumer _consumer;

        public GetPlatesConsumerTests()
        {
            _plateRepositoryMock = new Mock<IPlateRepository>();
            // AutoMapper configuration – replace with your actual mapping profiles.
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Plate, PlateDto>()
                    .ForMember(dest => dest.Registration, opt => opt.MapFrom(src => src.Registration));
            });
            _mapper = config.CreateMapper();
            _loggerMock = new Mock<ILogger<GetPlatesConsumer>>();
            _consumer = new GetPlatesConsumer(_loggerMock.Object, _plateRepositoryMock.Object, _mapper);
        }

        // Test: Consume_Should_RespondWithPlatesRetrievedEvent_When_PlatesExist
        // - Simulate GetAllPlatesAsync returning a list of plates
        [Fact]
        public async Task Consume_Should_RespondWithPlatesRetrievedEvent_When_PlatesExist()
        {
            // Arrange
            var plates = new List<Plate>
            {
                new Plate { Registration = "ABC123" }
            };
            _plateRepositoryMock.Setup(repo => repo.GetAllPlatesAsync()).ReturnsAsync(plates);

            var getPlatesEvent = new GetPlatesEvent { CorrelationId = Guid.NewGuid() };
            var contextMock = new Mock<ConsumeContext<GetPlatesEvent>>();
            contextMock.Setup(x => x.Message).Returns(getPlatesEvent);
            contextMock.Setup(x => x.RespondAsync(It.IsAny<PlatesRetrievedEvent>(), default))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            await _consumer.Consume(contextMock.Object);

            // Assert: Verify that RespondAsync was called with a PlatesRetrievedEvent containing one plate.
            contextMock.Verify(x => x.RespondAsync(
                It.Is<PlatesRetrievedEvent>(m => m.Plates != null && m.Plates.Count == 1 && m.Plates.First().Registration == "ABC123"),
                default), Times.Once);
        }

        // Test: Consume_Should_RespondWithEmptyPlates_When_PlatesDoNotExist
        // - Simulate GetAllPlatesAsync returning an empty list
        [Fact]
        public async Task Consume_Should_ThrowException_When_PlateRepositoryFails()
        {
            // Arrange
            _plateRepositoryMock.Setup(repo => repo.GetAllPlatesAsync()).ThrowsAsync(new Exception("Test error"));
            var getPlatesEvent = new GetPlatesEvent { CorrelationId = Guid.NewGuid() };
            var contextMock = new Mock<ConsumeContext<GetPlatesEvent>>();
            contextMock.Setup(x => x.Message).Returns(getPlatesEvent);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _consumer.Consume(contextMock.Object));
        }

        // Test: Consume_Should_RespondWithEmptyPlates_When_RepositoryReturnsEmptyList
        // - Simulate GetAllPlatesAsync returning an empty list
        [Fact]
        public async Task Consume_Should_RespondWithEmptyPlates_When_RepositoryReturnsEmptyList()
        {
            // Arrange
            var plates = new List<Plate>();
            _plateRepositoryMock.Setup(repo => repo.GetAllPlatesAsync()).ReturnsAsync(plates);

            var getPlatesEvent = new GetPlatesEvent { CorrelationId = Guid.NewGuid() };
            var contextMock = new Mock<ConsumeContext<GetPlatesEvent>>();
            contextMock.Setup(x => x.Message).Returns(getPlatesEvent);
            contextMock.Setup(x => x.RespondAsync(It.IsAny<PlatesRetrievedEvent>(), default))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            await _consumer.Consume(contextMock.Object);

            // Assert: Verify that RespondAsync was called with a PlatesRetrievedEvent containing an empty list.
            contextMock.Verify(x => x.RespondAsync(
                It.Is<PlatesRetrievedEvent>(m => m.Plates != null && !m.Plates.Any()),
                default), Times.Once);
        }

        // Test: Consume_Should_RespondWithEmptyPlates_When_RepositoryReturnsNull
        // - Simulate GetAllPlatesAsync returning null
        [Fact]
        public async Task Consume_Should_RespondWithEmptyPlates_When_RepositoryReturnsNull()
        {
            // Arrange
            List<Plate> plates = null;
            _plateRepositoryMock.Setup(repo => repo.GetAllPlatesAsync()).ReturnsAsync(plates);

            var getPlatesEvent = new GetPlatesEvent { CorrelationId = Guid.NewGuid() };
            var contextMock = new Mock<ConsumeContext<GetPlatesEvent>>();
            contextMock.Setup(x => x.Message).Returns(getPlatesEvent);
            contextMock.Setup(x => x.RespondAsync(It.IsAny<PlatesRetrievedEvent>(), default))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            await _consumer.Consume(contextMock.Object);

            // Assert: Verify that RespondAsync was called with a PlatesRetrievedEvent containing an empty list.
            contextMock.Verify(x => x.RespondAsync(
                It.Is<PlatesRetrievedEvent>(m => m.Plates != null && !m.Plates.Any()),
                default), Times.Once);
        }

        // Test: Consume_Should_Set_CorrelationId_From_Context
        // - Ensure that the CorrelationId in the response matches context.CorrelationId when it’s provided
        [Fact]
        public async Task Consume_Should_Set_CorrelationId_From_Context()
        {
            // Arrange
            var plates = new List<Plate>
            {
                new Plate { Registration = "ABC123" }
            };
            _plateRepositoryMock.Setup(repo => repo.GetAllPlatesAsync()).ReturnsAsync(plates);

            var correlationId = Guid.NewGuid();
            var getPlatesEvent = new GetPlatesEvent { CorrelationId = correlationId };
            var contextMock = new Mock<ConsumeContext<GetPlatesEvent>>();
            contextMock.Setup(x => x.Message).Returns(getPlatesEvent);
            contextMock.Setup(x => x.RespondAsync(It.IsAny<PlatesRetrievedEvent>(), default))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            await _consumer.Consume(contextMock.Object);

            // Assert: Verify that the response contains the correct CorrelationId.
            contextMock.Verify(x => x.RespondAsync(
                It.Is<PlatesRetrievedEvent>(m => m.CorrelationId == correlationId),
                default), Times.Once);
        }

        // Test: Consume_Should_RespondWith_Fault_When_ExceptionThrown
        // - Simulate an exception inside the try block
        [Fact]
        public async Task Consume_Should_RespondWith_Fault_When_ExceptionThrown()
        {
            // Arrange
            _plateRepositoryMock.Setup(repo => repo.GetAllPlatesAsync()).ThrowsAsync(new Exception("Test error"));
            var getPlatesEvent = new GetPlatesEvent { CorrelationId = Guid.NewGuid() };
            var contextMock = new Mock<ConsumeContext<GetPlatesEvent>>();
            contextMock.Setup(x => x.Message).Returns(getPlatesEvent);
            contextMock.Setup(x => x.RespondAsync(It.IsAny<Fault<PlatesRetrievedEvent>>(), default))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            await _consumer.Consume(contextMock.Object);

            // Assert: Verify that RespondAsync was called with a Fault message.
            contextMock.Verify(x => x.RespondAsync(
                It.Is<Fault<PlatesRetrievedEvent>>(m => m.Message != null),
                default), Times.Once);
        }

        // Test: Consume_Should_Not_Call_RespondAsync_When_Context_Is_Null
        // - Optional edge case: Pass null context (if your framework allows) and assert it throws ArgumentNullException
        [Fact]
        public async Task Consume_Should_Not_Call_RespondAsync_When_Context_Is_Null()
        {
            // Arrange
            var plates = new List<Plate>();
            _plateRepositoryMock.Setup(repo => repo.GetAllPlatesAsync()).ReturnsAsync(plates);

            // Act & Assert: Verify that an ArgumentNullException is thrown when context is null
            await Assert.ThrowsAsync<ArgumentNullException>(() => _consumer.Consume(null));
        }
    }
}

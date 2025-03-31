using System;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.API.Repositories;
using Catalog.API.Services;
using Catalog.Domain;
using Microsoft.Extensions.Logging;
using Moq;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;
using Xunit;

namespace Catalog.API.UnitTests.Services
{
    public class PlateServiceTests
    {
        private readonly Mock<IPlateRepository> _plateRepositoryMock;
        private readonly IMapper _mapper;
        private readonly ILogger<PlateService> _logger;
        private readonly PlateService _plateService;

        public PlateServiceTests()
        {
            _plateRepositoryMock = new Mock<IPlateRepository>();
            // Assuming you have an AutoMapper profile for Plate -> PlateDto mapping:
            var config = new MapperConfiguration(cfg =>
            {
                // Replace with your mapping profiles
                cfg.CreateMap<PlateDto, Plate>();
                cfg.CreateMap<Plate, PlateDto>();
            });
            _mapper = config.CreateMapper();
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            _logger = loggerFactory.CreateLogger<PlateService>();
            _plateService = new PlateService(_plateRepositoryMock.Object, _mapper, _logger);
        }

        // Test: AddPlateAsync_Should_CallRepository_When_ValidPlateDto
        // - Verify that IPlateRepository.AddPlateAsync is called once with the mapped Plate entity
        [Fact]
        public async Task AddPlateAsync_Should_CallRepository_When_ValidPlateDto()
        {
            // Arrange
            var plateDto = new PlateDto { Registration = "ABC123" };

            // Act
            await _plateService.AddPlateAsync(plateDto);

            // Assert: Verify that repository's AddPlateAsync was called once.
            _plateRepositoryMock.Verify(repo => repo.AddPlateAsync(It.IsAny<Plate>()), Times.Once);
        }

        // Test: AddPlateAsync_Should_ThrowArgumentException_When_RegistrationIsEmpty
        // - Simulate an empty registration string and verify that an ArgumentException is thrown
        [Fact]
        public async Task AddPlateAsync_Should_ThrowArgumentException_When_RegistrationIsEmpty()
        {
            // Arrange
            var plateDto = new PlateDto { Registration = "" };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _plateService.AddPlateAsync(plateDto));
        }

        // Test: AddPlateAsync_Should_Throw_When_Registration_Is_NullOrWhitespace
        // - Repeat for null, empty, and whitespace-only strings
        [Fact]
        public async Task AddPlateAsync_Should_Throw_When_Registration_Is_NullOrWhitespace()
        {
            // Arrange
            var plateDto = new PlateDto { Registration = null };

            // Act & Assert: Verify that an ArgumentException is thrown for null registration
            await Assert.ThrowsAsync<ArgumentException>(() => _plateService.AddPlateAsync(plateDto));

            // Arrange: Test with whitespace registration
            plateDto.Registration = "   ";

            // Act & Assert: Verify that an ArgumentException is thrown for whitespace registration
            await Assert.ThrowsAsync<ArgumentException>(() => _plateService.AddPlateAsync(plateDto));
        }

        // Test: AddPlateAsync_Should_Rethrow_When_RepositoryThrows
        // - Simulate _plateRepository.AddPlateAsync throwing an exception
        [Fact]
        public async Task AddPlateAsync_Should_Rethrow_When_RepositoryThrows()
        {
            // Arrange
            var plateDto = new PlateDto { Registration = "ABC123" };
            _plateRepositoryMock.Setup(repo => repo.AddPlateAsync(It.IsAny<Plate>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act & Assert: Verify that the exception is rethrown
            await Assert.ThrowsAsync<Exception>(() => _plateService.AddPlateAsync(plateDto));
        }

        // Test: AddPlateAsync_Should_Not_CallRepository_When_RegistrationIsInvalid
        // - If plate.Registration is whitespace/null, verify AddPlateAsync is not called
        [Fact]
        public async Task AddPlateAsync_Should_Not_CallRepository_When_RegistrationIsInvalid()
        {
            // Arrange
            var plateDto = new PlateDto { Registration = "   " }; // Invalid registration

            // Act
            await _plateService.AddPlateAsync(plateDto);

            // Assert: Verify that repository's AddPlateAsync was not called.
            _plateRepositoryMock.Verify(repo => repo.AddPlateAsync(It.IsAny<Plate>()), Times.Never);
        }

        // Optional: AddPlateAsync_Should_Handle_ValidPlateDto_With_ExtraFields
        // - Supply PlateDto with extra properties filled (PurchasePrice, SalePrice)
        [Fact]
        public async Task AddPlateAsync_Should_Handle_ValidPlateDto_With_ExtraFields()
        {
            // Arrange
            var plateDto = new PlateDto
            {
                Registration = "XYZ789",
                PurchasePrice = 1000,
                SalePrice = 1500
            };

            // Act
            await _plateService.AddPlateAsync(plateDto);

            // Assert: Verify that repository's AddPlateAsync was called once.
            _plateRepositoryMock.Verify(repo => repo.AddPlateAsync(It.IsAny<Plate>()), Times.Once);
        }

    }
}

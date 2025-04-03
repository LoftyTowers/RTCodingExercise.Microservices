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
using System.Collections.Generic;
using Catalog.Domain.Enums;
using System.Linq;

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
                cfg.CreateMap<Plate, PlateDto>()
                    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (EventBus.Enums.Status)src.StatusId))
                    .ForSourceMember(src => src.Status, opt => opt.DoNotValidate())
                    .ForSourceMember(src => src.AuditLogs, opt => opt.DoNotValidate());

                cfg.CreateMap<PlateDto, Plate>()
                    .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => (int)src.Status))
                    .ForMember(dest => dest.Status, opt => opt.Ignore())
                    .ForMember(dest => dest.AuditLogs, opt => opt.Ignore());
            });

            _mapper = config.CreateMapper();
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            _logger = loggerFactory.CreateLogger<PlateService>();
            _plateService = new PlateService(_plateRepositoryMock.Object, _mapper, _logger);
        }

        // Test: GetPlatesAsync_Should_Return_List_From_Repo
        // - Simulate _plateRepository.GetPlatesAsync returning a list of plates
        [Fact]
        public async Task GetPlatesAsync_Should_Return_List_From_Repo()
        {
            var plates = new List<Plate>
            {
                new Plate { Registration = "ABC123" }
            };

            var profitStats = new ProfitStats
            {
                AverageProfitMargin = 100,
                TotalRevenue = 150
            };

            _plateRepositoryMock
                .Setup(repo => repo.GetPlatesAsync(It.IsAny<SortField>(), It.IsAny<SortDirection>(), null, false))
                .ReturnsAsync(plates);

            _plateRepositoryMock
                .Setup(repo => repo.CalculateProfitStatsAsync())
                .ReturnsAsync(profitStats);

            var result = await _plateService.GetPlatesAsync(SortField.Registration, SortDirection.Ascending, null);

            Assert.NotNull(result);
            Assert.NotNull(result.Plates);
            Assert.Single(result.Plates);
            Assert.Equal("ABC123", result.Plates.ToList().FirstOrDefault()?.Registration);
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
    }
}

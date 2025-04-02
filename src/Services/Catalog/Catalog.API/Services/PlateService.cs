
using AutoMapper;
using Catalog.API.Repositories;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;
using Catalog.Domain;
using Catalog.Domain.Enums;

namespace Catalog.API.Services
{
    public class PlateService : IPlateService
    {
        private readonly IPlateRepository _plateRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PlateService> _logger;

        public PlateService(IPlateRepository plateRepository, IMapper mapper, ILogger<PlateService> logger)
        {
            _plateRepository = plateRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PlateDataDto> GetPlatesAsync(SortField field, SortDirection dir, string? filter = null, bool? onlyAvailable = false)
        {
            try
            {
                var plates = await _plateRepository.GetPlatesAsync(field, dir, filter, onlyAvailable);

                if (plates == null || !plates.Any())
                {
                    _logger.LogWarning("No plates found in the repository.");
                }

                var profitStats = await _plateRepository.CalculateProfitStatsAsync();

                return new PlateDataDto()
                {
                    Plates = _mapper.Map<List<PlateDto>>(plates),
                    AverageProfitMargin = profitStats.AverageProfitMargin,
                    TotalRevenue = profitStats.TotalRevenue
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in PlateService while retrieving plates.");
                throw;
            }
        }

        public async Task AddPlateAsync(PlateDto plateDto)
        {
            try
            {
                var plate = _mapper.Map<Plate>(plateDto);

                if (string.IsNullOrWhiteSpace(plate.Registration))
                    throw new ArgumentException("Plate registration cannot be null or empty.");

                await _plateRepository.AddPlateAsync(plate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in PlateService while adding a plate.");
                throw;
            }
        }

        public async Task UpdateStatusAsync(Plate plate)
        {
            try
            {
                await _plateRepository.UpdatePlateStatusAsync(plate);
                //await _auditRepository.LogAsync(plate, "Reserved");
                _logger.LogInformation("Plate {PlateId} reserved successfully.", plate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reserving plate {PlateId}", plate.Id);
                throw;
            }
        }

        public async Task SellPlateAsync(Plate plate)
        {
            try
            {
                _logger.LogInformation("Service: Routing SellPlateAsync for Plate ID {Id}", plate.Id);
                await _plateRepository.SellPlateAsync(plate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error selling plate {PlateId}", plate.Id);
                throw;
            }
        }

    }
}
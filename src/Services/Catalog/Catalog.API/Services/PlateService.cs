
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

        public async Task<IEnumerable<PlateDto>> GetPlatesAsync(SortField field, SortDirection dir, string? filter = null)
        {
            try
            {
                var plates = await _plateRepository.GetPlatesAsync(field, dir, filter);

                if (plates == null || !plates.Any())
                {
                    _logger.LogWarning("No plates found in the repository.");
                }

                var plateDtos = _mapper.Map<List<PlateDto>>(plates);
                return plateDtos;
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
    }
}
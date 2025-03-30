
using AutoMapper;
using Catalog.API.Repositories;
using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;
using Catalog.Domain;

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
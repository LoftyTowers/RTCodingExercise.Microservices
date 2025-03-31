using Catalog.API.Repositories;

namespace Catalog.API.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IPlateRepository _plateRepository;
        private readonly IAuditRepository _auditRepository;
        private readonly ILogger<ReservationService> _logger;

        public ReservationService(
            IPlateRepository plateRepository,
            IAuditRepository auditRepository,
            ILogger<ReservationService> logger)
        {
            _plateRepository = plateRepository;
            _auditRepository = auditRepository;
            _logger = logger;
        }

        public async Task ReservePlateAsync(Guid plateId)
        {
            try
            {
                await _plateRepository.UpdateStatusAsync(plateId, "Reserved");
                await _auditRepository.LogAsync(plateId, "Reserved");
                _logger.LogInformation("Plate {PlateId} reserved successfully.", plateId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reserving plate {PlateId}", plateId);
                throw;
            }
        }

        public async Task UnreservePlateAsync(Guid plateId)
        {
            try
            {
                await _plateRepository.UpdateStatusAsync(plateId, "Available");
                await _auditRepository.LogAsync(plateId, "Unreserved");
                _logger.LogInformation("Plate {PlateId} unreserved successfully.", plateId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error unreserving plate {PlateId}", plateId);
                throw;
            }
        }
    }
}

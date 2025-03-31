using Catalog.API.Repositories;

namespace Catalog.API.Services
{
    public class SalesService : ISalesService
    {
        private readonly IPlateRepository _plateRepository;
        private readonly ILogger<SalesService> _logger;

        public SalesService(IPlateRepository plateRepository, ILogger<SalesService> logger)
        {
            _plateRepository = plateRepository;
            _logger = logger;
        }

        public async Task SellPlateAsync(Guid plateId)
        {
            try
            {
                await _plateRepository.UpdateStatusAsync(plateId, "Sold");
                _logger.LogInformation("Plate {PlateId} marked as sold.", plateId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error selling plate {PlateId}", plateId);
                throw;
            }
        }

        public async Task<ProfitStats> CalculateProfitStatsAsync()
        {
            try
            {
                var stats = await _plateRepository.CalculateProfitStatsAsync();
                _logger.LogInformation("Profit stats calculated successfully.");
                return stats;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating profit statistics.");
                throw;
            }
        }
    }
}

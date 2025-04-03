using Catalog.API.Helpers;
using Catalog.Domain.Enums;

namespace Catalog.API.Repositories
{
    public class PlateRepository : IPlateRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PlateRepository> _logger;

        public PlateRepository(ILogger<PlateRepository> logger, ApplicationDbContext context)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            if (context == null) throw new ArgumentNullException(nameof(context));
            {
                _context = context;
            }
        }

        public async Task<IEnumerable<Plate>> GetPlatesAsync(SortField field, SortDirection direction, string? filter = null, bool? onlyAvailable = false)
        {
            try
            {
                _logger.LogInformation("Getting plates with sort {SortField} {SortDirection} and filter '{Filter}'", field, direction, filter);

                var query = _context.Plates
                    .AsQueryable()
                    .ApplyBroadVisualFilter(filter);

                if (onlyAvailable.HasValue && onlyAvailable.Value)
                {
                    query = query.Where(p => p.StatusId == (int)Domain.Enums.Status.Available);

                }

                var result = await query.ApplySort(field, direction).ToListAsync();

                return result.ApplyVisualPrecision(filter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve plates from the database.");
                throw;
            }
        }

        public async Task<Plate> AddPlateAsync(Plate plate)
        {
            try
            {
                if (plate == null) throw new ArgumentNullException(nameof(plate));
                if (string.IsNullOrWhiteSpace(plate.Registration))
                    throw new ArgumentException("Plate registration cannot be null or empty.", nameof(plate.Registration));

                _context.Plates.Add(plate);
                await _context.SaveChangesAsync();
                return plate;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving plate to the database.");
                throw;
            }
        }

        public async Task UpdatePlateStatusAsync(Plate plate)
        {
            try
            {
                _logger.LogDebug($"Plate promo code: {plate.PromoCodeUsed}, Plate final sales price: {plate.FinalSalePrice}, plate statusId: {plate.StatusId}");
                _context.Entry(plate).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred updateing status of plate: {plate}.", plate.Id);
                throw;
            }
        }

        public async Task<bool> DeletePlateAsync(Guid id)
        {
            var plate = await _context.Plates.FindAsync(id);
            if (plate == null) return false;

            _context.Plates.Remove(plate);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<ProfitStats> CalculateProfitStatsAsync()
        {
            try
            {
                var soldPlates = await _context.Plates.CountAsync(p => p.StatusId == (int)Domain.Enums.Status.Sold);
                var totalPlates = await _context.Plates.CountAsync();

                return new ProfitStats
                {
                    TotalRevenue = soldPlates * 100,
                    AverageProfitMargin = totalPlates > 0 ? (decimal)soldPlates / totalPlates : 0
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting profit from the db.");
                throw;
            }
        }

        public async Task SellPlateAsync(Plate plate)
        {
            try
            {
                var existing = await _context.Plates.FindAsync(plate.Id);
                if (existing == null)
                {
                    _logger.LogWarning("SellPlateAsync: Plate not found with ID {Id}", plate.Id);
                    return;
                }

                existing.StatusId = plate.StatusId;
                existing.FinalSalePrice = plate.FinalSalePrice;
                existing.PromoCodeUsed = plate.PromoCodeUsed;

                await _context.SaveChangesAsync();

                _logger.LogInformation("SellPlateAsync: Updated Plate ID {Id} with status {Status} and final sale price {Price}.",
                    existing.Id, existing.StatusId, existing.FinalSalePrice);
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while marking a plate as sold, ID {Id}", plate.Id);
                throw;
            }
        }
    }
}

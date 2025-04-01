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

        public async Task<IEnumerable<Plate>> GetPlatesAsync(SortField field, SortDirection direction, string? filter = null)
        {
            try
            {
                _logger.LogInformation("Getting plates with sort {SortField} {SortDirection} and filter '{Filter}'", field, direction, filter);

                var query = _context.Plates
                    .AsQueryable()
                    .ApplyBroadVisualFilter(filter)
                    .ApplySort(field, direction);

                var result = await query.ToListAsync();

                return result.ApplyVisualPrecision(filter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve plates from the database.");
                throw;
            }
        }

        public async Task<Plate?> GetPlateByIdAsync(Guid id)
        {
            return await _context.Plates.FindAsync(id);
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

        public async Task<bool> UpdatePlateAsync(Plate plate)
        {
            _context.Entry(plate).State = EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;
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
            // var soldPlates = await _context.Plates.CountAsync(p => p.Status == "Sold");
            // var totalPlates = await _context.Plates.CountAsync();

            return new ProfitStats
            {
                // TotalRevenue = soldPlates * 100, 
                // AverageProfitMargin = totalPlates > 0 ? (decimal)soldPlates / totalPlates : 0
            };
        }

        public async Task UpdateStatusAsync(Guid plateId, string status)
        {
            var plate = await GetPlateByIdAsync(plateId);
            if (plate == null) throw new ArgumentNullException(nameof(plate));

            //plate.Status = status;
            await UpdatePlateAsync(plate);
        }

        public async Task ApplyFlatDiscountAsync(decimal discountAmount)
        {
            var plates = await _context.Plates.ToListAsync();
            foreach (var plate in plates)
            {
                plate.SalePrice -= discountAmount;
                _context.Plates.Update(plate);
            }
            await _context.SaveChangesAsync();
        }

        public async Task ApplyPercentDiscountAsync(decimal discountPercentage)
        {
            var plates = await _context.Plates.ToListAsync();
            foreach (var plate in plates)
            {
                plate.SalePrice -= plate.SalePrice * discountPercentage;
                _context.Plates.Update(plate);
            }
            await _context.SaveChangesAsync();
        }
    }
}

using Catalog.Domain;
using Catalog.API.Data;

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

        public async Task<IEnumerable<Plate>> GetAllPlatesAsync()
        {
            return await _context.Plates.ToListAsync();
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
    }
}

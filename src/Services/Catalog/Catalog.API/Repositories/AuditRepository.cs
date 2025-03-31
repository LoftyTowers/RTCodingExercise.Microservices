using Catalog.Domain;
using Catalog.API.Data;

namespace Catalog.API.Repositories
{
    public class AuditRepository : IAuditRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PlateRepository> _logger;

        public AuditRepository(ILogger<PlateRepository> logger, ApplicationDbContext context)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            if (context == null) throw new ArgumentNullException(nameof(context));
            {
                _context = context;
            }
        }


        public async Task LogAsync(Guid plateId, string action)
        {
            var log = new AuditLog
            {
                PlateId = plateId,
                Action = action,
                Timestamp = DateTime.UtcNow
            };

            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}

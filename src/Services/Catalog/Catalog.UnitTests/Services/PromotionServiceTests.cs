using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Catalog.API.Services;
using Catalog.API.Repositories;
using System.Threading.Tasks;

namespace Catalog.UnitTests.Services
{
    public class PromotionServiceTests
    {
        private readonly Mock<IPlateRepository> _mockPlateRepo;
        private readonly Mock<IAuditRepository> _mockAuditRepo;
        private readonly Mock<ILogger<PromotionService>> _mockLogger;
        private readonly PromotionService _service;

        public PromotionServiceTests()
        {
            _mockPlateRepo = new Mock<IPlateRepository>();
            _mockAuditRepo = new Mock<IAuditRepository>();
            _mockLogger = new Mock<ILogger<PromotionService>>();
            _service = new PromotionService(_mockPlateRepo.Object, _mockLogger.Object);
        }
    }
}

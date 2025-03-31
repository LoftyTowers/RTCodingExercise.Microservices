using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Catalog.API.Services;
using Catalog.API.Repositories;
using Catalog.Domain;
using System.Threading.Tasks;

namespace Catalog.UnitTests.Services
{
    public class SalesServiceTests
    {
        private readonly Mock<IPlateRepository> _mockPlateRepo;
        private readonly Mock<IAuditRepository> _mockAuditRepo;
        private readonly Mock<ILogger<SalesService>> _mockLogger;
        private readonly SalesService _service;

        public SalesServiceTests()
        {
            _mockPlateRepo = new Mock<IPlateRepository>();
            _mockAuditRepo = new Mock<IAuditRepository>();
            _mockLogger = new Mock<ILogger<SalesService>>();
            _service = new SalesService(_mockPlateRepo.Object, _mockLogger.Object);
        }
    }
}

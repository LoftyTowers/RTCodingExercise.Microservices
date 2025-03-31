using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Catalog.API.Services;
using Catalog.API.Repositories;
using Catalog.Domain;
using System.Threading.Tasks;

namespace Catalog.UnitTests.Services
{
    public class ReservationServiceTests
    {
        private readonly Mock<IPlateRepository> _mockPlateRepo;
        private readonly Mock<IAuditRepository> _mockAuditRepo;
        private readonly Mock<ILogger<ReservationService>> _mockLogger;
        private readonly ReservationService _service;

        public ReservationServiceTests()
        {
            _mockPlateRepo = new Mock<IPlateRepository>();
            _mockAuditRepo = new Mock<IAuditRepository>();
            _mockLogger = new Mock<ILogger<ReservationService>>();
            _service = new ReservationService(_mockPlateRepo.Object, _mockAuditRepo.Object, _mockLogger.Object);
        }
    }
}

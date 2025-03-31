using RTCodingExercise.Microservices.Models;

namespace RTCodingExercise.Microservices.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly IPlateQueryService _plateQueryService;
        private readonly IPlateCommandService _plateCommandService;
        private readonly ILogger<ReservationsController> _logger;

        public ReservationsController(
            IPlateQueryService plateQueryService,
            IPlateCommandService plateCommandService,
            ILogger<ReservationsController> logger)
        {
            _plateQueryService = plateQueryService;
            _plateCommandService = plateCommandService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> ReservePlate(PlateViewModel plate)
        {
            try
            {
                _logger.LogInformation("Attempting to reserve plate with ID: {Id}", plate.Id);
                await _plateCommandService.ReservePlateAsync(plate);
                _logger.LogInformation("Successfully reserved plate with ID: {Id}", plate.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to reserve plate with ID: {Id}", plate.Id);
                TempData["ErrorMessage"] = "Could not reserve plate.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UnreservePlate(PlateViewModel plate)
        {
            try
            {
                _logger.LogInformation("Attempting to unreserve plate with ID: {Id}", plate.Id);
                await _plateCommandService.UnreservePlateAsync(plate);
                _logger.LogInformation("Successfully unreserved plate with ID: {Id}", plate.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to unreserve plate with ID: {Id}", plate.Id);
                TempData["ErrorMessage"] = "Could not unreserve plate.";
            }

            return RedirectToAction("Index");
        }
    }
}

using RTCodingExercise.Microservices.WebMVC.Services;
using RTCodingExercise.Microservices.Models;

namespace RTCodingExercise.Microservices.Controllers
{
    public class ReservationsController : Controller
    {
        #region Fields

        private readonly IPlateQueryService _plateQueryService;
        private readonly IPlateCommandService _plateCommandService;
        private readonly ILogger<PlatesController> _logger;

        #endregion

        public ReservationsController(IPlateQueryService plateQueryService, IPlateCommandService plateCommandService, ILogger<PlatesController> logger)
        {
            _plateQueryService = plateQueryService;
            _plateCommandService = plateCommandService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> ReservePlate(int id)
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UnreservePlate(int id)
        {
            return RedirectToAction("Index");
        }
    }
}
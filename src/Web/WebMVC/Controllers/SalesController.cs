using RTCodingExercise.Microservices.WebMVC.Services;
using RTCodingExercise.Microservices.Models;

namespace RTCodingExercise.Microservices.Controllers
{
    public class SalesController : Controller
    {
        #region Fields

        private readonly IPlateQueryService _plateQueryService;
        private readonly IPlateCommandService _plateCommandService;
        private readonly ILogger<PlatesController> _logger;

        #endregion

        public SalesController(IPlateQueryService plateQueryService, IPlateCommandService plateCommandService, ILogger<PlatesController> logger)
        {
            _plateQueryService = plateQueryService;
            _plateCommandService = plateCommandService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> SellPlate(int id)
        {
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> CalculateProfitMargin()
        {
            return View("Index");
        }
    }
}
using RTCodingExercise.Microservices.WebMVC.Services;
using RTCodingExercise.Microservices.Models;

namespace RTCodingExercise.Microservices.Controllers
{
    public class PromotionsController : Controller
    {
        #region Fields

        private readonly IPlateQueryService _plateQueryService;
        private readonly IPlateCommandService _plateCommandService;
        private readonly ILogger<PlatesController> _logger;

        #endregion

        public PromotionsController(IPlateQueryService plateQueryService, IPlateCommandService plateCommandService, ILogger<PlatesController> logger)
        {
            _plateQueryService = plateQueryService;
            _plateCommandService = plateCommandService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> ApplyDiscount(string promoCode)
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ApplyPercentOff(string promoCode)
        {
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ValidateDiscount(string promoCode, int plateId)
        {
            return View("Index");
        }
    }
}
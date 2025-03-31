using RTCodingExercise.Microservices.WebMVC.Services;
using RTCodingExercise.Microservices.Models;

namespace RTCodingExercise.Microservices.Controllers
{
    public class PlatesController : Controller
    {
        #region Fields

        private readonly IPlateQueryService _plateQueryService;
        private readonly IPlateCommandService _plateCommandService;
        private readonly ILogger<PlatesController> _logger;

        #endregion

        public PlatesController(IPlateQueryService plateQueryService, IPlateCommandService plateCommandService, ILogger<PlatesController> logger)
        {
            _plateQueryService = plateQueryService;
            _plateCommandService = plateCommandService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 20)
        {
            try
            {
                _logger.LogInformation("Sending request for plates via MassTransit.");

                // Send the MassTransit request to get plates
                var allPlates = await _plateQueryService.GetPlatesAsync();

                _logger.LogInformation($"Received {allPlates.Count} plates from the Catalog API.");

                // Apply pagination to the retrieved plates
                var pagedPlates = allPlates
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                _logger.LogInformation($"Displaying page {page} with page size {pageSize}.");

                // Calculate pagination details
                ViewBag.CurrentPage = page;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalPages = (int)Math.Ceiling((double)allPlates.Count / pageSize);

                return View(pagedPlates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching plates.");
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddPlate(string registration, decimal purchasePrice)
        {
            try
            {
                if (string.IsNullOrEmpty(registration) || purchasePrice <= 0)
                {
                    ModelState.AddModelError("", "Invalid input data.");
                    return RedirectToAction("Index");
                }

                var plate = new PlateViewModel
                {
                    Registration = registration.ToUpper(),
                    PurchasePrice = purchasePrice,
                    SalePrice = purchasePrice * 1.2m
                };

                await _plateCommandService.AddPlateAsync(plate);

                _logger.LogInformation("Plate added: {Plate}", plate.Registration);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding plate.");
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> OrderByPrice()
        {
            return View("Index");
        }

        public async Task<IActionResult> FilterByNumber(string number)
        {
            return View("Index");
        }

        public async Task<IActionResult> FilterByLetter(string letter)
        {
            return View("Index");
        }

        public async Task<IActionResult> FilterByName(string name)
        {
            return View("Index");
        }
        

        public async Task<IActionResult> ForSaleOnly()
        {
            return View("Index");
        }
    }
}

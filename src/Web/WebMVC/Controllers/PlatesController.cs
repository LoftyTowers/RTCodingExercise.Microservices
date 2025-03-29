// using RTCodingExercise.Microservices.Services;
 using RTCodingExercise.Microservices.Models;

namespace RTCodingExercise.Microservices.Controllers
{
    public class PlatesController : Controller
    {
        #region Fields

        private readonly ILogger<PlatesController> _logger;
        //private readonly IPlateService _plateService;

        #endregion


        // public PlatesController(IPlateService plateService)
        // {
        //     _plateService = plateService;
        // }

        public PlatesController(ILogger<PlatesController> logger)
        {
            _logger = logger;
        }
        
        // Populate some sample data for now
        private static List<PlateViewModel> plates = new List<PlateViewModel>
        {
            new PlateViewModel { Registration = "ABC 123", PurchasePrice = 500, SalePrice = 600 },
            new PlateViewModel { Registration = "XYZ 789", PurchasePrice = 700, SalePrice = 840 },
            new PlateViewModel { Registration = "MNO 456", PurchasePrice = 400, SalePrice = 480 },
            new PlateViewModel { Registration = "PET 125", PurchasePrice = 500, SalePrice = 600 }
        };

        public IActionResult Index(int page = 1, int pageSize = 20)
        {
            try
            {
                // Simulate some processing
                _logger.LogInformation("Fetching plates from the database.");
                // var plates = await _plateService.GetAllPlatesAsync();
                _logger.LogInformation($"Displaying the page {page} of the list of plates.");
                var pagedPlates = plates
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = (int)Math.Ceiling((double)plates.Count / pageSize);

                return View(pagedPlates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching plates.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public IActionResult AddPlate(string registration, decimal purchasePrice)
        {
            try
            {
                if (string.IsNullOrEmpty(registration) || purchasePrice <= 0)
                {
                    ModelState.AddModelError("", "Invalid input data.");
                    return View("Index", plates);
                }
                else
                {
                    var newPlate = new PlateViewModel
                    {
                        Registration = registration.ToUpper(),
                        PurchasePrice = purchasePrice,
                        SalePrice = purchasePrice * 1.2m // Example sale price calculation
                    };
                    plates.Add(newPlate);
                    _logger.LogInformation($"New plate added: {registration}");
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a plate.");
                return StatusCode(500, "Internal server error");
            }
            
        }

        // public async Task<IActionResult> Index(int page = 1, int pageSize = 20)
        // {
        //     var plates = await _plateService.GetPagedPlatesAsync(page, pageSize);
        //     return View(plates);
        // }

        // [HttpPost]
        // public async Task<IActionResult> AddPlate(Plate plate)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         await _plateService.AddPlateAsync(plate);
        //         return RedirectToAction("Index");
        //     }
        //     return View(plate);
        // }
    }
}

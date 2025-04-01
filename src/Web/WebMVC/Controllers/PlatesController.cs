using RTCodingExercise.Microservices.Models;
using WebMVC.Enums;
using System.Text.RegularExpressions;

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

        public async Task<IActionResult> Index(int page = 1, int pageSize = 20, SortField field = SortField.None, SortDirection direction = SortDirection.Ascending)
        {
            try
            {
                _logger.LogInformation("Sending request for plates via MassTransit.");

                // Get all plates
                var allPlates = (await _plateQueryService.GetSortedPlatesAsync(field, direction)).ToList();

                _logger.LogInformation($"Received {allPlates.Count} plates from the Catalog API.");

                // Guard against invalid pageSize
                pageSize = pageSize <= 0 ? 20 : pageSize;

                // Calculate pagination
                var totalPlates = allPlates.Count;
                var totalPages = (int)Math.Ceiling(totalPlates / (double)pageSize);
                page = Math.Clamp(page, 1, Math.Max(totalPages, 1)); // prevent overflow or underflow

                var pagedPlates = allPlates
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                _logger.LogInformation($"Displaying page {page} with page size {pageSize}.");

                // Set view data
                ViewBag.CurrentPage = page;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalPages = totalPages;
                ViewBag.CurrentSortField = field;
                ViewBag.CurrentSortDirection = direction;

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

        [HttpGet]
        public async Task<IActionResult> Filter(string? query, int page = 1, int pageSize = 20)
        {
            try
            {
                _logger.LogInformation("Filtering plates with query: {Query}", query);

                if (!string.IsNullOrWhiteSpace(query) && !Regex.IsMatch(query, @"^[A-Za-z0-9 ]+$"))
                {
                    _logger.LogWarning("Invalid characters detected in query: {Query}", query);
                    TempData["FilterError"] = "Please enter only letters and numbers (e.g. 'JAMES' or 'TAG44'). Special characters are not allowed.";
                    return RedirectToAction("Index");
                }

                var filteredPlates = await _plateQueryService.FilterPlatesAsync(query);

                var pagedPlates = filteredPlates
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                ViewBag.CurrentPage = page;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalPages = (int)Math.Ceiling((double)Math.Max(filteredPlates.Count(), 1) / pageSize);
                ViewBag.Query = query;

                return View("Index", pagedPlates); // Reuse the Index view
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while filtering plates.");
                return RedirectToAction("Error", "Home");
            }
        }
        

        public async Task<IActionResult> ForSaleOnly()
        {
            return View("Index");
        }
    }
}

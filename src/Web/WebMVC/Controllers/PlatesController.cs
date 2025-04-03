using RTCodingExercise.Microservices.Models;
using WebMVC.Enums;
using System.Text.RegularExpressions;

namespace RTCodingExercise.Microservices.Controllers
{
    public class PlatesController : Controller
    {
        #region Fields

        private readonly IPlateQueryService _plateQueryService;
        private readonly ILogger<PlatesController> _logger;

        #endregion

        public PlatesController(IPlateQueryService plateQueryService, ILogger<PlatesController> logger)
        {
            _plateQueryService = plateQueryService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 20, SortField field = SortField.None, SortDirection direction = SortDirection.Ascending)
        {
            try
            {
                _logger.LogInformation("Sending request for plates via MassTransit.");

                var allPlateData = await _plateQueryService.GetSortedPlatesAsync(field, direction);
                _logger.LogInformation($"Received {allPlateData.Plates.Count} plates from the Catalog API.");

                pageSize = pageSize <= 0 ? 20 : pageSize;
                var totalPlates = allPlateData.Plates.Count;
                var totalPages = (int)Math.Ceiling(totalPlates / (double)pageSize);
                page = Math.Clamp(page, 1, Math.Max(totalPages, 1));

                // Apply pagination to just the Plates list
                allPlateData.Plates = allPlateData.Plates
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                ViewBag.CurrentPage = page;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalPages = totalPages;
                ViewBag.CurrentSortField = field;
                ViewBag.CurrentSortDirection = direction;

                return View(allPlateData);
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
                    return await Index(); // fallback to existing index logic
                }

                var plate = new PlateViewModel
                {
                    Registration = registration.ToUpper(),
                    PurchasePrice = purchasePrice,
                    SalePrice = purchasePrice * 1.2m
                };

                var updatedModel = await _plateQueryService.AddPlateAsync(plate);
                _logger.LogInformation("Plate added: {Plate}", plate.Registration);

                return View("Index", updatedModel); // ✅ Return the view with updated model directly
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding plate.");
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Filter(string? query, bool onlyAvailable = false, int page = 1, int pageSize = 20, SortField field = SortField.None, SortDirection direction = SortDirection.Ascending)
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

                var filteredPlateData = await _plateQueryService.FilterPlatesAsync(query, onlyAvailable);

                // Apply sorting
                filteredPlateData.Plates = direction == SortDirection.Ascending
                    ? filteredPlateData.Plates.OrderBy(p => GetSortValue(p, field)).ToList()
                    : filteredPlateData.Plates.OrderByDescending(p => GetSortValue(p, field)).ToList();

                // Pagination
                var totalPlates = filteredPlateData.Plates.Count;
                var totalPages = (int)Math.Ceiling(totalPlates / (double)pageSize);
                page = Math.Clamp(page, 1, Math.Max(totalPages, 1));
                filteredPlateData.Plates = filteredPlateData.Plates
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                // ViewBag for persistence
                ViewBag.CurrentPage = page;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalPages = totalPages;
                ViewBag.Query = query;
                ViewBag.OnlyAvailable = onlyAvailable;
                ViewBag.CurrentSortField = field;
                ViewBag.CurrentSortDirection = direction;

                return View("Index", filteredPlateData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while filtering plates.");
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(PlateViewModel plate)
        {
            try
            {
                // Normalize values
                if (plate.FinalSalePrice.HasValue)
                {
                    plate.FinalSalePrice = null;
                }

                if (!string.IsNullOrWhiteSpace(plate.PromoCodeUsed))
                {
                    plate.PromoCodeUsed = null;
                }

                var updatedModel = await _plateQueryService.UpdateStatusAsync(plate);
                return View("Index", updatedModel); // ✅ Renders updated page
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to toggle reservation for plate ID: {Id}", plate?.Id);
                TempData["ErrorMessage"] = "Could not update reservation status. Please try again.";
                return RedirectToAction("Error", "Home");
            }
        }


        private object? GetSortValue(PlateViewModel plate, SortField field)
        {
            return field switch
            {
                SortField.Registration => plate.Registration,
                SortField.PurchasePrice => plate.PurchasePrice,
                SortField.SalePrice => plate.SalePrice,
                _ => null
            };
        }
    }
}

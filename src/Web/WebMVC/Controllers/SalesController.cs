using RTCodingExercise.Microservices.WebMVC.Services;
using RTCodingExercise.Microservices.Models;
using WebMVC.Enums;

namespace RTCodingExercise.Microservices.Controllers
{
    public class SalesController : Controller
    {
        #region Fields

        private readonly ISalesQueryService _saleQueryService;
        private readonly ILogger<PlatesController> _logger;

        #endregion

        public SalesController(ISalesQueryService saleQueryService, ILogger<PlatesController> logger)
        {
            _saleQueryService = saleQueryService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> SellPlate(PlateViewModel plate, string? promoCode)
        {
            try
            {
                if (plate == null || plate.Id == Guid.Empty)
                {
                    TempData["ErrorMessage"] = "Invalid plate data.";
                    return RedirectToAction("Index", "Plates");
                }

                decimal discounted = plate.SalePrice;

                if (!string.IsNullOrWhiteSpace(promoCode))
                {
                    if (promoCode.Equals("DISCOUNT", StringComparison.OrdinalIgnoreCase))
                    {
                        _logger.LogDebug("We Are -£25");
                        discounted -= 25m;

                        _logger.LogDebug($"[DISCOUNT LOG] Sales Price: {plate.SalePrice}, Discounted price: {discounted}, Price Difference: {plate.SalePrice * 0.9m}");

                        if (discounted < (plate.SalePrice * 0.9m))
                        {
                            TempData["ErrorMessage"] = "Discount too large. Cannot sell below 90% of original sale price.";
                            return RedirectToAction("Index", "Plates");
                        }
                    }
                    else if (promoCode.Equals("PERCENTOFF", StringComparison.OrdinalIgnoreCase))
                    {
                        _logger.LogDebug("We Are -£15");
                        discounted *= 0.85m;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Invalid Discount code. Enter a valid code or continue without.";
                            return RedirectToAction("Index", "Plates");
                    }
                }

                plate.FinalSalePrice = discounted;
                plate.PromoCodeUsed = promoCode;
                plate.Status = Status.Sold;

                await _saleQueryService.SellPlate(plate);
                TempData["SuccessMessage"] = $"Plate sold for {discounted:C}.";
                return RedirectToAction("Index", "Plates");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to sell plate.");
                TempData["ErrorMessage"] = "Something went wrong while processing the sale.";
                return RedirectToAction("Index", "Plates");
            }
        }
    }
}
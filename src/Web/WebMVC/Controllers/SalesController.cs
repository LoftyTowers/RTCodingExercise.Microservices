using RTCodingExercise.Microservices.WebMVC.Services;
using RTCodingExercise.Microservices.Models;
using WebMVC.Enums;

namespace RTCodingExercise.Microservices.Controllers
{
    public class SalesController : Controller
    {
        #region Fields

        private readonly IPlateQueryService _plateQueryService;
        private readonly ISalesCommandService _saleCommandService;
        private readonly ILogger<PlatesController> _logger;

        #endregion

        public SalesController(IPlateQueryService plateQueryService, ISalesCommandService saleCommandService, ILogger<PlatesController> logger)
        {
            _plateQueryService = plateQueryService;
            _saleCommandService = saleCommandService;
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
                        discounted -= 25;
                    else if (promoCode.Equals("PERCENTOFF", StringComparison.OrdinalIgnoreCase))
                        discounted *= 0.85m;
                }

                if (discounted < plate.SalePrice * 0.9m)
                {
                    TempData["ErrorMessage"] = "Discount too large. Cannot sell below 90% of original sale price.";
                    return RedirectToAction("Index", "Plates");
                }

                plate.FinalSalePrice = discounted;
                plate.PromoCodeUsed = promoCode;
                plate.Status = Status.Sold;

                await _saleCommandService.SellPlate(plate);
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
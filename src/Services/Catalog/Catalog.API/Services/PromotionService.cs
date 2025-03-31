using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Catalog.API.Repositories;

namespace Catalog.API.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly IPlateRepository _plateRepository;
        private readonly ILogger<PromotionService> _logger;

        public PromotionService(IPlateRepository plateRepository, ILogger<PromotionService> logger)
        {
            _plateRepository = plateRepository;
            _logger = logger;
        }

        public async Task ApplyDiscountAsync(string promoCode)
        {
            try
            {
                if (promoCode == "DISCOUNT")
                {
                    await _plateRepository.ApplyFlatDiscountAsync(25);
                    _logger.LogInformation("£25 discount applied to eligible plates.");
                }
                else
                {
                    _logger.LogWarning("Invalid promo code received: {PromoCode}", promoCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error applying discount for promo code: {PromoCode}", promoCode);
                throw;
            }
        }

        public async Task ApplyPercentOffAsync(string promoCode)
        {
            try
            {
                if (promoCode == "PERCENTOFF")
                {
                    await _plateRepository.ApplyPercentDiscountAsync(0.15m);
                    _logger.LogInformation("15%% discount applied to eligible plates.");
                }
                else
                {
                    _logger.LogWarning("Invalid promo code received: {PromoCode}", promoCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error applying percent discount for promo code: {PromoCode}", promoCode);
                throw;
            }
        }
    }
}

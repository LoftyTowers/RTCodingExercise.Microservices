namespace Catalog.API.Services
{
    public interface IPromotionService
    {
        Task ApplyDiscountAsync(string promoCode);
        Task ApplyPercentOffAsync(string promoCode);
    }
}
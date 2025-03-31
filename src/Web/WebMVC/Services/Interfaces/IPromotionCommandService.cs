public interface IPromotionCommandService
{
    Task ApplyDiscountAsync(string promoCode);
    Task ApplyPercentOffAsync(string promoCode);
}
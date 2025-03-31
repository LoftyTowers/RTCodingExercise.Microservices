namespace Catalog.API.Services
{

    public interface ISalesService
    {
        Task<ProfitStats> CalculateProfitStatsAsync();
        Task SellPlateAsync(Guid plateId);
    }
}
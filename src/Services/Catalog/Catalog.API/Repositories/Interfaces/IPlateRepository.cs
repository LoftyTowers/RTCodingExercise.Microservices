using Catalog.Domain.Enums;

namespace Catalog.API.Repositories
{
    public interface IPlateRepository
    {
        Task<IEnumerable<Plate>> GetPlatesAsync(SortField field, SortDirection dir, string? filter = null, bool? onlyAvailable = false);
        Task<Plate> AddPlateAsync(Plate plate);
        Task UpdatePlateStatusAsync(Plate plate);
        Task<ProfitStats> CalculateProfitStatsAsync();
        Task SellPlateAsync(Plate plate);
    }
}

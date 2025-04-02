using Catalog.Domain.Enums;

namespace Catalog.API.Repositories
{
    public interface IPlateRepository
    {
        Task<IEnumerable<Plate>> GetPlatesAsync(SortField field, SortDirection dir, string? filter = null);
        Task<Plate?> GetPlateByIdAsync(Guid id);
        Task<Plate> AddPlateAsync(Plate plate);
        Task<bool> UpdatePlateAsync(Plate plate);
        Task<bool> DeletePlateAsync(Guid id);
        Task<ProfitStats> CalculateProfitStatsAsync();
        Task UpdateStatusAsync(Plate plate);
        Task ApplyFlatDiscountAsync(decimal discountAmount);
        Task ApplyPercentDiscountAsync(decimal discountPercentage);
    }
}

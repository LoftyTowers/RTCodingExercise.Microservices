using Catalog.Domain.Enums;

namespace Catalog.API.Repositories
{
    public interface IPlateRepository
    {
        Task<IEnumerable<Plate>> GetAllPlatesAsync(SortField field, SortDirection dir);
        Task<Plate?> GetPlateByIdAsync(Guid id);
        Task<Plate> AddPlateAsync(Plate plate);
        Task<bool> UpdatePlateAsync(Plate plate);
        Task<bool> DeletePlateAsync(Guid id);
        Task<ProfitStats> CalculateProfitStatsAsync();
        Task UpdateStatusAsync(Guid plateId, string status);
        Task ApplyFlatDiscountAsync(decimal discountAmount);
        Task ApplyPercentDiscountAsync(decimal discountPercentage);
    }
}

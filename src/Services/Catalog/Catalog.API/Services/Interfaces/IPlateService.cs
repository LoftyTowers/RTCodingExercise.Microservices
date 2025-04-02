using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;
using Catalog.Domain.Enums;

namespace Catalog.API.Services
{
    public interface IPlateService
    {
        Task<IEnumerable<PlateDto>> GetPlatesAsync(SortField field, SortDirection dir, string? filter = null);
        Task AddPlateAsync(PlateDto plateDto);
        Task UpdateStatusAsync(Plate plate);
    }
}
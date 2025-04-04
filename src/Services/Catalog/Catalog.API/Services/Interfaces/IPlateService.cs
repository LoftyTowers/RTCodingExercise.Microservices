using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;
using Catalog.Domain.Enums;

namespace Catalog.API.Services
{
    public interface IPlateService
    {
        Task<PlateDataDto> GetPlatesAsync(SortField field, SortDirection dir, string? filter = null, bool? onlyAvailable = false);
        Task<PlateDataDto> AddPlateAsync(PlateDto plateDto);
        Task<PlateDataDto> UpdateStatusAsync(Plate plate);
        Task<PlateDataDto> SellPlateAsync(Plate plate);
    }
}
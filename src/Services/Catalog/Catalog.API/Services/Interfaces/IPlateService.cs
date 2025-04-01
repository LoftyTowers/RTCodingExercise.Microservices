using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;
using Catalog.Domain.Enums;

namespace Catalog.API.Services
{
    public interface IPlateService
    {
        Task<IEnumerable<PlateDto>> GetAllPlatesAsync(SortField field, SortDirection dir);
        Task AddPlateAsync(PlateDto plateDto);
    }
}
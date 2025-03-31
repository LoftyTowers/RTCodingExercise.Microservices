using RTCodingExercise.Microservices.BuildingBlocks.EventBus.IntegrationEvents.Models;

namespace Catalog.API.Services
{
    public interface IPlateService
    {
        Task AddPlateAsync(PlateDto plateDto);
    }
}
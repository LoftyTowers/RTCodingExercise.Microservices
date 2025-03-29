using RTCodingExercise.Microservices.Models;
public interface IPlateQueryService
{
    Task<List<PlateViewModel>> GetPlatesAsync();
}
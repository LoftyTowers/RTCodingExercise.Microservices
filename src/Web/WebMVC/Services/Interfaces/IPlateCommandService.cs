using RTCodingExercise.Microservices.Models;
public interface IPlateCommandService
{
    Task AddPlateAsync(PlateViewModel plate);
}
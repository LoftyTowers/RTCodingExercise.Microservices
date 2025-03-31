using RTCodingExercise.Microservices.Models;
public interface IPlateCommandService
{
    Task AddPlateAsync(PlateViewModel plate);
    Task ReservePlateAsync(PlateViewModel plate);
    Task UnreservePlateAsync(PlateViewModel plate);
}
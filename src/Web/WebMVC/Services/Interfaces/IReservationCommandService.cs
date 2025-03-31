
using RTCodingExercise.Microservices.Models;

public interface IReservationCommandService
{
    Task ReservePlateAsync(PlateViewModel plate);
    Task UnreservePlateAsync(PlateViewModel plate);
}
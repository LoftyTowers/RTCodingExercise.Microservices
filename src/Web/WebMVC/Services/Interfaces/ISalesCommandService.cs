using RTCodingExercise.Microservices.Models;

public interface ISalesCommandService
{
    Task SellPlateAsync(PlateViewModel plate);
}
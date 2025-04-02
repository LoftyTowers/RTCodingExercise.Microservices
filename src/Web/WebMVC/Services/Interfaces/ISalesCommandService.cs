using RTCodingExercise.Microservices.Models;

public interface ISalesCommandService
{
    Task SellPlate(PlateViewModel plate);

}
using RTCodingExercise.Microservices.Models;

public interface ISalesQueryService
{
    Task<PlateDataViewModel> SellPlate(PlateViewModel plate);

}
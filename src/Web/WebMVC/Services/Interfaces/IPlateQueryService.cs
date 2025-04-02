using RTCodingExercise.Microservices.Models;
using WebMVC.Enums;
public interface IPlateQueryService
{
    Task<PlateDataViewModel> GetSortedPlatesAsync(SortField field, SortDirection direction);
    Task<PlateDataViewModel> FilterPlatesAsync(string filter, bool onlyAvailable);
}
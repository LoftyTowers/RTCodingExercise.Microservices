using RTCodingExercise.Microservices.Models;
using WebMVC.Enums;
public interface IPlateQueryService
{
    Task<IEnumerable<PlateViewModel>> GetSortedPlatesAsync(SortField field, SortDirection direction);
}
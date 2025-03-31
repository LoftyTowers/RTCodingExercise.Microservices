
namespace Catalog.API.Services
{
    public interface IReservationService
    {
        Task ReservePlateAsync(Guid plateId);
        Task UnreservePlateAsync(Guid plateId);
    }
}
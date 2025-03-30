using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Domain;

namespace Catalog.API.Repositories
{
    public interface IPlateRepository
    {
        Task<IEnumerable<Plate>> GetAllPlatesAsync();
        Task<Plate?> GetPlateByIdAsync(Guid id);
        Task<Plate> AddPlateAsync(Plate plate);
        Task<bool> UpdatePlateAsync(Plate plate);
        Task<bool> DeletePlateAsync(Guid id);
    }
}

using System.Linq.Expressions;
using Catalog.Domain.Enums;

namespace Catalog.API.Repositories.Helpers
{
    public static class PlateSortExpressions
    {
        public static readonly Dictionary<SortField, Expression<Func<Plate, object>>> Map =
            new()
            {
                { SortField.SalePrice, p => p.SalePrice },
                { SortField.PurchasePrice, p => p.PurchasePrice },
                { SortField.Registration, p => p.Registration },
                { SortField.None, p => p.Id } // default fallback
            };
    }
}

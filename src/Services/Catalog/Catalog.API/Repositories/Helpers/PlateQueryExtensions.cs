using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Catalog.Domain.Enums;

namespace Catalog.API.Helpers
{
    public static class PlateQueryExtensions
    {
        public static readonly Dictionary<SortField, Expression<Func<Plate, object>>> SortMap = new()
        {
            { SortField.SalePrice, p => p.SalePrice },
            { SortField.PurchasePrice, p => p.PurchasePrice },
            { SortField.Registration, p => p.Registration },
        };

        public static IQueryable<Plate> ApplySort(this IQueryable<Plate> query, SortField field, SortDirection direction)
        {
            if (!SortMap.TryGetValue(field, out var selector))
                return query;

            return direction == SortDirection.Ascending
                ? query.OrderBy(selector)
                : query.OrderByDescending(selector);
        }

        public static IQueryable<Plate> ApplyBroadVisualFilter(this IQueryable<Plate> query, string? filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
                return query;

            var variants = PlateVisualMatcher.GetVisualVariants(filter);

            // Build the OR expression tree
            Expression<Func<Plate, bool>> predicate = p => false;
            foreach (var v in variants)
            {
                var variant = v; // avoid closure issue
                predicate = predicate.Or(p =>
                    (p.Registration != null && p.Registration.ToUpper().Contains(variant)) ||
                    (p.Letters != null && p.Letters.ToUpper().Contains(variant)) ||
                    p.Numbers.ToString().Contains(variant));
            }

            return query.Where(predicate);
        }

        public static IEnumerable<Plate> ApplyVisualPrecision(this IEnumerable<Plate> plates, string? filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
                return plates;

            var pattern = PlateVisualMatcher.ToRegexPattern(filter.ToUpperInvariant());

            return plates.Where(p =>
                p.Registration != null && Regex.IsMatch(p.Registration.ToUpper(), pattern));
        }

        // Predicate builder for OR chaining
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T));

            var left = Expression.Invoke(expr1, parameter);
            var right = Expression.Invoke(expr2, parameter);
            var orElse = Expression.OrElse(left, right);

            return Expression.Lambda<Func<T, bool>>(orElse, parameter);
        }
    }
}

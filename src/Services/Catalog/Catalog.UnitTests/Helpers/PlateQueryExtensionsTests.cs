using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Catalog.API.Helpers;
using Catalog.Domain;
using Catalog.Domain.Enums;
using Moq;
using Xunit;

namespace Catalog.UnitTests.Helpers
{
    public class PlateQueryExtensionsTests
    {
        private List<Plate> GetSamplePlates() => new()
        {
            new Plate { Registration = "JAM3S", Letters = "JAM", Numbers = 3, PurchasePrice = 1000, SalePrice = 1500 },
            new Plate { Registration = "DA44NY", Letters = "DAN", Numbers = 44, PurchasePrice = 800, SalePrice = 1400 },
            new Plate { Registration = "XYZ123", Letters = "XYZ", Numbers = 123, PurchasePrice = 500, SalePrice = 900 },
        };

        [Fact]
        public void ApplySort_ReturnsSortedPlates_BySalePriceAscending()
        {
            var plates = GetSamplePlates().AsQueryable();
            var sorted = plates.ApplySort(SortField.SalePrice, SortDirection.Ascending).ToList();

            Assert.Equal("XYZ123", sorted.First().Registration);
            Assert.Equal("JAM3S", sorted.Last().Registration);
        }

        [Fact]
        public void ApplySort_ReturnsSortedPlates_ByPurchasePriceDescending()
        {
            var plates = GetSamplePlates().AsQueryable();
            var sorted = plates.ApplySort(SortField.PurchasePrice, SortDirection.Descending).ToList();

            Assert.Equal("JAM3S", sorted.First().Registration);
            Assert.Equal("XYZ123", sorted.Last().Registration);
        }

        [Fact]
        public void ApplySort_ReturnsOriginal_WhenSortFieldInvalid()
        {
            var plates = GetSamplePlates().AsQueryable();
            var sorted = plates.ApplySort(SortField.None, SortDirection.Ascending);

            Assert.Equal(plates, sorted);
        }

        [Fact]
        public void ApplyBroadVisualFilter_FiltersUsingVariants()
        {
            var plates = GetSamplePlates().AsQueryable();
            var filtered = plates.ApplyBroadVisualFilter("James").ToList();

            Assert.Single(filtered);
            Assert.Equal("JAM3S", filtered.First().Registration);
        }

        [Fact]
        public void ApplyBroadVisualFilter_ReturnsAll_WhenFilterIsEmpty()
        {
            var plates = GetSamplePlates().AsQueryable();
            var result = plates.ApplyBroadVisualFilter(null);

            Assert.Equal(plates, result);
        }

        [Fact]
        public void ApplyVisualPrecision_ReturnsOnlyMatchingRegex()
        {
            var plates = GetSamplePlates();
            var result = plates.ApplyVisualPrecision("James").ToList();

            Assert.Single(result);
            Assert.Equal("JAM3S", result.First().Registration);
        }

        [Fact]
        public void ApplyVisualPrecision_ReturnsAll_WhenFilterIsEmpty()
        {
            var plates = GetSamplePlates();
            var result = plates.ApplyVisualPrecision(null).ToList();

            Assert.Equal(plates.Count, result.Count);
        }
    }
}

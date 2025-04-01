using System.Collections.Generic;
using System.Linq;
using Catalog.API.Helpers;
using Xunit;

namespace Catalog.UnitTests.Helpers
{
    public class PlateVisualMatcherTests
    {
        [Theory]
        [InlineData("James", "[J][A4][M][E3][S5]")]
        [InlineData("BOLT", "[B8][O0][L][T7]")]
        [InlineData("GIZMO", "[G69][I1][Z2][M][O0]")]
        public void ToRegexPattern_CreatesExpectedPattern(string input, string expected)
        {
            var result = PlateVisualMatcher.ToRegexPattern(input);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToRegexPattern_SkipsSymbols()
        {
            var result = PlateVisualMatcher.ToRegexPattern("JA-ME$");
            Assert.Equal("[J][A4][M][E3]", result);
        }

        [Fact]
        public void GetVisualVariants_ReturnsExpectedVariants()
        {
            var variants = PlateVisualMatcher.GetVisualVariants("James");

            Assert.Contains("JAMES", variants);
            Assert.Contains("J4MES", variants);
            Assert.Contains("JAM3S", variants);
        }

        [Fact]
        public void GetVisualVariants_ReturnsEmpty_ForNullOrWhitespace()
        {
            var result1 = PlateVisualMatcher.GetVisualVariants(null);
            var result2 = PlateVisualMatcher.GetVisualVariants("   ");

            Assert.Empty(result1);
            Assert.Empty(result2);
        }
    }
}
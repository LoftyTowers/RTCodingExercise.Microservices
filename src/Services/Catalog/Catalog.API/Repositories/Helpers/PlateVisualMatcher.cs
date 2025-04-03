using System.Text;
using System.Text.RegularExpressions;

namespace Catalog.API.Helpers
{
    public static class PlateVisualMatcher
    {
        // Single source of substitutions
        private static readonly Dictionary<char, char[]> SubstitutionMap = new()
        {
            { 'A', new[] { '4' } },
            { 'B', new[] { '8' } },
            { 'E', new[] { '3' } },
            { 'G', new[] { '6', '9' } },
            { 'I', new[] { '1' } },
            { 'O', new[] { '0' } },
            { 'S', new[] { '5' } },
            { 'T', new[] { '7' } },
            { 'Z', new[] { '2' } }
        };

        /// <summary>
        /// Generates a regex pattern that allows letter-digit visual substitutions (e.g., A = 4).
        /// </summary>
        public static string ToRegexPattern(string input)
        {
            var sb = new StringBuilder();

            foreach (var ch in input.ToUpperInvariant())
            {
                if (!char.IsLetterOrDigit(ch)) continue;

                if (SubstitutionMap.TryGetValue(ch, out var subs))
                {
                    sb.Append('[').Append(ch);
                    foreach (var s in subs)
                        sb.Append(s);
                    sb.Append(']');
                }
                else
                {
                    sb.Append(ch);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns a list of basic visual substitution variants to help widen SQL filtering.
        /// </summary>
        public static List<string> GetVisualVariants(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return new List<string>();

            var upper = input.ToUpperInvariant();
            var variants = new HashSet<string> { upper };

            foreach (var kvp in SubstitutionMap)
            {
                foreach (var sub in kvp.Value)
                {
                    var replaced = upper.Replace(kvp.Key, sub);
                    variants.Add(replaced);
                }
            }

            return variants.ToList();
        }
    }
}
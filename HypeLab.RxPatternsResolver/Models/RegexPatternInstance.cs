using System;
using System.Text.RegularExpressions;

namespace HypeLab.RxPatternsResolver.Models
{
    internal class RegexPatternInstance : IEquatable<RegexPatternInstance>
    {
        public string Pattern { get; set; } = null!;
        public string Replacement { get; set; } = null!;
        public RegexOptions RegexOption { get; set; }

        public override int GetHashCode()
        {
            return HashCode.Combine(Pattern, Replacement, (int)RegexOption);
        }

        public override bool Equals(object? obj)
        {
            if (obj is RegexPatternInstance rxPatterninstance)
                return Equals(rxPatterninstance);

            return false;
        }

        public bool Equals(RegexPatternInstance? other)
        {
            if (other is null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return Pattern == other.Pattern && Replacement == other.Replacement && RegexOption == other.RegexOption;
        }

        public static bool operator ==(RegexPatternInstance? left, RegexPatternInstance? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(RegexPatternInstance? left, RegexPatternInstance? right)
        {
            return !Equals(left, right);
        }
    }
}

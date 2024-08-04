using System.Text.RegularExpressions;

namespace HypeLab.RxPatternsResolver.Models
{
    internal class RegexPatternInstance
    {
        public string Pattern { get; set; } = null!;
        public string Replacement { get; set; } = null!;
        public RegexOptions RegexOption { get; set; }
    }
}

using Microsoft.Extensions.DependencyInjection;

namespace HypeLab.RxPatternsResolver
{
    /// <summary>
    /// Extension methods
    /// </summary>
    public static class RegexResolverExtensions
    {
        /// <summary>
        /// Adds a singleton RegexPatternsResolver type to the specified IServiceCollection.
        /// </summary>
        public static void AddRegexResolver(this IServiceCollection services)
        {
            services.AddSingleton<RegexPatternsResolver>();
        }
    }
}

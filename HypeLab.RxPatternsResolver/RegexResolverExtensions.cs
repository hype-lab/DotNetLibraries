using HypeLab.DnsLookupClient;
using HypeLab.DnsLookupClient.Data.Interfaces;
using HypeLab.RxPatternsResolver.Helpers;
using HypeLab.RxPatternsResolver.Interfaces;
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
            services.AddSingleton<ILookupClient, LookupClient>(implementationFactory: _ => new LookupClient());

            services.AddSingleton<IEmailChecker, EmailChecker>(serviceProvider =>
            {
                ILookupClient lookUpClient = serviceProvider.GetRequiredService<ILookupClient>();

                return new EmailChecker(lookUpClient);
            });

            services.AddSingleton(serviceProvider =>
            {
                IEmailChecker emailChecker = serviceProvider.GetRequiredService<IEmailChecker>();
                return new RegexPatternsResolver(emailChecker);
            });
        }
    }
}

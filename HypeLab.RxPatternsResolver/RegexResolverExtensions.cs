using HypeLab.RxPatternsResolver.Constants;
using HypeLab.RxPatternsResolver.Helpers;
using HypeLab.RxPatternsResolver.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

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
            services.AddHttpClient(RxResolverDefaults.EmailCheckerHttpClientName);

            services.AddSingleton<IEmailChecker, EmailChecker>(serviceProvider =>
            {
                IHttpClientFactory httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
                HttpClient httpClient = httpClientFactory.CreateClient(RxResolverDefaults.EmailCheckerHttpClientName);
                return new EmailChecker(httpClient);
            });

            services.AddSingleton(serviceProvider =>
            {
                IEmailChecker emailChecker = serviceProvider.GetRequiredService<IEmailChecker>();
                return new RegexPatternsResolver(emailChecker);
            });
        }
    }
}

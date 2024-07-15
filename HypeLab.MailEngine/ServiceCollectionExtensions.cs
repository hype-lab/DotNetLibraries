using HypeLab.MailEngine.Factories;
using HypeLab.MailEngine.Services.Impl;
using HypeLab.MailEngine.Services;
using Microsoft.Extensions.DependencyInjection;
using HypeLab.MailEngine.Data.Models;
using HypeLab.MailEngine.Data.Exceptions;
using HypeLab.MailEngine.Data.Models.Impl;
using HypeLab.MailEngine.Data.Enums;
using HypeLab.MailEngine.Helpers;
using HypeLab.MailEngine.Factories.Impl;

namespace HypeLab.MailEngine
{
    /// <summary>
    /// Extension methods for the IServiceCollection interface.
    /// For now the engine registers services as scoped apart of the sendgrid options builder, but this design can be changed in the future.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the mail engine to the service collection.
        /// For now the engine registers services as scoped apart of the sendgrid options builder, but this design can be changed in the future.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="mailAccessInfo"></param>
        /// <returns></returns>
        /// <exception cref="InvalidEmailSenderTypeException"></exception>
        public static IServiceCollection AddMailEngine(this IServiceCollection services, IMailAccessInfo mailAccessInfo)
        {
            ArgumentNullException.ThrowIfNull(mailAccessInfo);
            MailAccessInfoClientIdNullException.ThrowIfClientIdNullOrEmpty(mailAccessInfo);

            services.AddScopedMailEngine(mailAccessInfo, isSingleSender: true);

            services.AddScoped<IEmailSenderFactory, EmailSenderFactory>();
            services.AddScoped<IEmailService, EmailService>(serviceProvider =>
            {
                // uses the service provider to get the IMailAccessInfo and IEmailSenderFactory instances
                return new EmailService(serviceProvider.GetRequiredService<IMailAccessInfo>(), serviceProvider.GetRequiredService<IEmailSenderFactory>());
            });

            return services;
        }

        /// <summary>
        /// Adds the mail engine to the service collection.
        /// For now the engine registers services as scoped, but this design can be changed in the future.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="mailAccessInfoParams"></param>
        /// <returns></returns>
        /// <exception cref="DefaultEmailSenderNotFoundException"></exception>
        /// <exception cref="InvalidEmailSenderTypeException"></exception>
        public static IServiceCollection AddMailEngine(this IServiceCollection services, params IMailAccessInfo[] mailAccessInfoParams)
        {
            ArgumentNullException.ThrowIfNull(mailAccessInfoParams);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(mailAccessInfoParams.Length);
            MultipleDefaultEmailSendersFoundException.ThrowIfMultipleDefaultEmailSenders(mailAccessInfoParams);
            DuplicateClientIdNamesException.ThrowIfDuplicateClientIdNames(mailAccessInfoParams);
            MailAccessInfoClientIdNullException.ThrowIfClientIdNullOrEmpty(mailAccessInfoParams);

            EmailSenderType defaultSenderType = mailAccessInfoParams.SingleOrDefault(x => x.IsDefault && x.EmailSenderType != EmailSenderType.Unknown)?.EmailSenderType
                ?? throw new DefaultEmailSenderNotFoundException("Email sender di default non trovato.");

            services.AddScoped<IMailAccessesInfo>(_ => new MailAccessesInfo(defaultSenderType, mailAccessInfoParams));

            foreach (IMailAccessInfo mailAccessInfo in mailAccessInfoParams)
            {
                services.AddScopedMailEngine(mailAccessInfo);
            }

            services.AddScoped<IEmailSenderFactory, EmailSenderFactory>();
            services.AddScoped<IEmailService, EmailService>(serviceProvider =>
            {
                // uses the service provider to get the IMailAccessesInfo and IEmailSenderFactory instances
                return new EmailService(serviceProvider.GetRequiredService<IMailAccessesInfo>(), serviceProvider.GetRequiredService<IEmailSenderFactory>());
            });

            return services;
        }
    }
}

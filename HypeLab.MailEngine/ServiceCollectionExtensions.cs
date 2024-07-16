using Microsoft.Extensions.DependencyInjection;
using HypeLab.MailEngine.Data.Models;
using HypeLab.MailEngine.Data.Exceptions;
using HypeLab.MailEngine.Data.Models.Impl;
using HypeLab.MailEngine.Data.Enums;
using HypeLab.MailEngine.Helpers;
using HypeLab.MailEngine.Helpers.Const;

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

            services.AddKeyedScopedMailEngine(mailAccessInfo, isSingleSender: true);

            services.AddEmailSenderFactory();
            services.AddSingleInfoAccessEmailService();

            return services;
        }

        /// <summary>
        /// Adds the mail engine to the service collection.
        /// For now the engine registers services as scoped apart of the sendgrid options builder, but this design can be changed in the future.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="mailAccessInfoParams"></param>
        /// <returns></returns>
        /// <exception cref="DefaultEmailSenderNotFoundException"></exception>
        public static IServiceCollection AddMailEngine(this IServiceCollection services, params IMailAccessInfo[] mailAccessInfoParams)
        {
            ArgumentNullException.ThrowIfNull(mailAccessInfoParams);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(mailAccessInfoParams.Length);
            MultipleDefaultEmailSendersFoundException.ThrowIfMultipleDefaultEmailSenders(mailAccessInfoParams);
            DuplicateClientIdNamesException.ThrowIfDuplicateClientIdNames(mailAccessInfoParams);
            MailAccessInfoClientIdNullException.ThrowIfClientIdNullOrEmpty(mailAccessInfoParams);

            EmailSenderType defaultSenderType = mailAccessInfoParams.SingleOrDefault(x => x.IsDefault && x.EmailSenderType != EmailSenderType.Unknown)?.EmailSenderType
                ?? throw new DefaultEmailSenderNotFoundException(ExceptionDefaults.DefaultEmailSenderNotFound.DefaultMessage);

            services.AddScoped<IMailAccessesInfo>(_ => new MailAccessesInfo(defaultSenderType, mailAccessInfoParams));

            foreach (IMailAccessInfo mailAccessInfo in mailAccessInfoParams)
            {
                services.AddKeyedScopedMailEngine(mailAccessInfo);
            }

            services.AddEmailSenderFactory();
            services.AddMultipleInfoAccessEmailService();

            return services;
        }
    }
}

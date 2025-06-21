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
        /// Adds the mail engine services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <remarks>This method registers the necessary services for sending emails. It ensures that the mail engine  is
        /// configured for single-sender scenarios.</remarks>
        /// <param name="services">The <see cref="IServiceCollection"/> to which the mail engine services will be added.</param>
        /// <param name="mailAccessInfo">The mail access information required to configure the mail engine. Cannot be <see langword="null"/> and must
        /// contain a valid client ID.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="mailAccessInfo"/> is <see langword="null"/>.</exception>
        /// <exception cref="MailAccessInfoClientIdNullException">Thrown if the <paramref name="mailAccessInfo"/> has a null or empty client ID.</exception>
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
        /// Adds the mail engine services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <remarks>After validation, the method registers the mail engine services, including keyed mail engines for each  <see cref="IMailAccessInfo"/>.
        /// </remarks>
        /// <param name="services">The <see cref="IServiceCollection"/> to which the mail engine services will be added.</param>
        /// <param name="mailAccessInfoParams">An array of <see cref="IMailAccessInfo"/> objects that define the configuration for mail access.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="mailAccessInfoParams"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="mailAccessInfoParams"/> is empty or contains zero elements.</exception>
        /// <exception cref="MultipleDefaultEmailSendersFoundException">Thrown if multiple default email senders are found in <paramref name="mailAccessInfoParams"/>.</exception>
        /// <exception cref="DuplicateClientIdNamesException">Thrown if there are duplicate client ID names in <paramref name="mailAccessInfoParams"/>.</exception>
        /// <exception cref="MailAccessInfoClientIdNullException">Thrown if any <see cref="IMailAccessInfo"/> in <paramref name="mailAccessInfoParams"/> has a null or empty client ID.</exception>
        /// <exception cref="DefaultEmailSenderNotFoundException">Thrown if no default email sender is configured in <paramref name="mailAccessInfoParams"/>.</exception>
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

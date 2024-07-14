using HypeLab.MailEngine.Data.Models.Impl.Credentials;
using HypeLab.MailEngine.Factories.Impl;
using HypeLab.MailEngine.Factories;
using HypeLab.MailEngine.Services.Impl;
using HypeLab.MailEngine.Services;
using HypeLab.MailEngine.Strategies.EmailSender.Impl;
using HypeLab.MailEngine.Strategies.EmailSender;
using Microsoft.Extensions.DependencyInjection;
using SendGrid.Extensions.DependencyInjection;
using HypeLab.MailEngine.Data.Models;
using HypeLab.MailEngine.Data.Exceptions;
using HypeLab.MailEngine.Data.Models.Impl;
using HypeLab.MailEngine.Data.Enums;
using Newtonsoft.Json;
using HypeLab.MailEngine.SmtpClients;

namespace HypeLab.MailEngine
{
    /// <summary>
    /// Extension methods for the IServiceCollection interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the mail engine to the service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="mailAccessInfo"></param>
        /// <returns></returns>
        /// <exception cref="InvalidEmailSenderTypeException"></exception>
        public static IServiceCollection AddMailEngine(this IServiceCollection services, IMailAccessInfo mailAccessInfo)
        {
            ArgumentNullException.ThrowIfNull(mailAccessInfo);
            MailAccessInfoClientIdNullException.ThrowIfClientIdNullOrEmpty("ClientId non può essere nullo o vuoto.", mailAccessInfo);

            switch (mailAccessInfo)
            {
                case SmtpAccessInfo smtpAccessInfo:
                    services.AddScoped<IMailAccessInfo>(_ => new SmtpAccessInfo(smtpAccessInfo.ClientId, smtpAccessInfo.Server, smtpAccessInfo.Port, smtpAccessInfo.AccountEmail, smtpAccessInfo.Password, smtpAccessInfo.EnableSsl, smtpAccessInfo.IsDefault));
                    services.AddKeyedScoped<ISmtpEmailSender, SmtpEmailSender>(serviceKey: smtpAccessInfo.ClientId, implementationFactory: (_, __) => new SmtpEmailSender(new CustomSmtpClient(new SmtpAccessInfo(smtpAccessInfo.ClientId, smtpAccessInfo.Server, smtpAccessInfo.Port, smtpAccessInfo.AccountEmail, smtpAccessInfo.Password, smtpAccessInfo.EnableSsl, smtpAccessInfo.IsDefault))));
                    break;
                case SendGridAccessInfo sgAccessInfo:
                    services.AddScoped<IMailAccessInfo>(_ => new SendGridAccessInfo(sgAccessInfo.ClientId, sgAccessInfo.ApiKey, sgAccessInfo.IsDefault));
                    services.AddSendGrid(options =>
                    {
                        options.ApiKey = sgAccessInfo.ApiKey;
                        options.RequestHeaders = new Dictionary<string, string>
                        {
                            { "X-Client-Id", sgAccessInfo.ClientId }
                        };
                    });
                    services.AddKeyedScoped<ISendGridEmailSender, SendGridEmailSender>(serviceKey: sgAccessInfo.ClientId);
                    break;
                default:
                    throw new InvalidEmailSenderTypeException($"Invalid email sender type: {mailAccessInfo.GetType()}");
            }

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
            MultipleDefaultEmailSendersFoundException.ThrowIfMultipleDefaultEmailSenders(mailAccessInfoParams, "Multipli default email sender trovati.\nÈ possibile impostare solo un email sender di default.");
            DuplicateClientIdNamesException.ThrowIfDuplicateClientIdNames(mailAccessInfoParams, "Nomi client id duplicati trovati.");
            MailAccessInfoClientIdNullException.ThrowIfClientIdNullOrEmpty("ClientId non può essere nullo o vuoto.", mailAccessInfoParams);

            EmailSenderType defaultSenderType = mailAccessInfoParams.SingleOrDefault(x => x.IsDefault && x.EmailSenderType != EmailSenderType.Unknown)?.EmailSenderType
                ?? throw new DefaultEmailSenderNotFoundException("Email sender di default non trovato.");

            services.AddScoped<IMailAccessesInfo>(_ => new MailAccessesInfo(defaultSenderType, mailAccessInfoParams));

            foreach (IMailAccessInfo mailAccessInfo in mailAccessInfoParams)
            {
                string mailAccessInfoType = JsonConvert.SerializeObject(mailAccessInfo.GetType());

                switch (mailAccessInfo)
                {
                    case SmtpAccessInfo smtpAccessInfo:
                        services.AddKeyedScoped<ISmtpEmailSender, SmtpEmailSender>(serviceKey: smtpAccessInfo.ClientId, implementationFactory: (_, __) => new SmtpEmailSender(new CustomSmtpClient(new SmtpAccessInfo(smtpAccessInfo.ClientId, smtpAccessInfo.Server, smtpAccessInfo.Port, smtpAccessInfo.AccountEmail, smtpAccessInfo.Password, smtpAccessInfo.EnableSsl, smtpAccessInfo.IsDefault))));
                        break;
                    case SendGridAccessInfo sgAccessInfo:
                        services.AddSendGrid(options =>
                        {
                            options.ApiKey = sgAccessInfo.ApiKey;
                            options.RequestHeaders = new Dictionary<string, string>
                            {
                                { "X-Client-Id", sgAccessInfo.ClientId }
                            };
                        });
                        services.AddKeyedScoped<ISendGridEmailSender, SendGridEmailSender>(serviceKey: sgAccessInfo.ClientId);
                        break;
                    default:
                        throw new InvalidEmailSenderTypeException($"L'email sender fornito è di tipo invalido: {mailAccessInfoType}");
                }
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

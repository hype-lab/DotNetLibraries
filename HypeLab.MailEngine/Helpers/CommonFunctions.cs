using HypeLab.MailEngine.Data.Exceptions;
using HypeLab.MailEngine.Data.Models.Impl.Credentials;
using HypeLab.MailEngine.Data.Models;
using HypeLab.MailEngine.Factories;
using HypeLab.MailEngine.Helpers.Providers.SendGrid.Impl;
using HypeLab.MailEngine.Helpers.Providers.SendGrid;
using HypeLab.MailEngine.SmtpClients;
using HypeLab.MailEngine.Strategies.EmailSender;
using HypeLab.MailEngine.Strategies.EmailSender.Impl;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SendGrid;
using HypeLab.MailEngine.SendGrid;
using HypeLab.MailEngine.Factories.Impl;
using HypeLab.MailEngine.Services.Impl;
using HypeLab.MailEngine.Services;

namespace HypeLab.MailEngine.Helpers
{
    internal static class CommonFunctions
    {
        public static IEmailSender DetermineSenderByClientId(this string clientId, IEmailSenderFactory emailSenderFactory)
        {
            ArgumentException.ThrowIfNullOrEmpty(clientId);

            return emailSenderFactory.CreateEmailSender(clientId);
        }

        public static void AddScopedMailEngine(this IServiceCollection services, IMailAccessInfo mailAccessInfo)
        {
            switch (mailAccessInfo)
            {
                case SmtpAccessInfo smtpAccessInfo:
                    services.AddKeyedScoped<CustomSmtpClient, CustomSmtpClient>(serviceKey: smtpAccessInfo.ClientId, implementationFactory: (serviceProvider, _) =>
                    {
                        if (serviceProvider.GetRequiredService<IMailAccessInfo>() is SmtpAccessInfo smtpAccInf)
                            return new CustomSmtpClient(smtpAccInf);

                        throw new InvalidEmailSenderTypeException("Tipo di email sender non valido.");
                    });

                    services.AddKeyedScoped<ISmtpEmailSender, SmtpEmailSender>(serviceKey: smtpAccessInfo.ClientId, implementationFactory: (serviceProvider, _) =>
                    {
                        CustomSmtpClient customSmtpClient = serviceProvider.GetRequiredKeyedService<CustomSmtpClient>(smtpAccessInfo.ClientId);
                        return new SmtpEmailSender(customSmtpClient);
                    });
                    break;
                case SendGridAccessInfo sgAccessInfo:
                    services.AddSingleton<ISendGridOptionsProvider, SendGridOptionsProvider>(_ =>
                    {
                        SendGridOptionsProvider provider = new();
                        provider.Configure(sgAccessInfo.ClientId, new SendGridClientOptions
                        {
                            ApiKey = sgAccessInfo.ApiKey,
                            RequestHeaders = new Dictionary<string, string>
                            {
                                { "X-Client-Id", sgAccessInfo.ClientId }
                            }
                        });
                        return provider;
                    });

                    services.AddHttpClient(sgAccessInfo.ClientId, httpClient => httpClient.Timeout = TimeSpan.FromSeconds(100));

                    services.AddKeyedScoped<ISendGridClient>(serviceKey: sgAccessInfo.ClientId, implementationFactory: (serviceProvider, __) =>
                    {
                        IHttpClientFactory httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
                        HttpClient httpClient = httpClientFactory.CreateClient(sgAccessInfo.ClientId); // crea HttpClient usando il nome univoco

                        ISendGridOptionsProvider optionsProvider = serviceProvider.GetRequiredService<ISendGridOptionsProvider>();
                        SendGridClientOptions clientOptions = optionsProvider.GetOptions(sgAccessInfo.ClientId);
                        return new CustomSendGridClient(httpClient, clientOptions);
                    });

                    services.AddKeyedScoped<ISendGridEmailSender, SendGridEmailSender>(serviceKey: sgAccessInfo.ClientId, implementationFactory: (serviceProvider, _) =>
                    {
                        ISendGridClient customSendGridClient = serviceProvider.GetRequiredKeyedService<CustomSendGridClient>(sgAccessInfo.ClientId);
                        return new SendGridEmailSender(customSendGridClient);
                    });
                    break;
                default:
                    throw new InvalidEmailSenderTypeException($"L'email sender fornito è di tipo invalido: {JsonConvert.SerializeObject(mailAccessInfo.GetType())}");
            }

            services.AddScoped<IEmailSenderFactory, EmailSenderFactory>();
        }

        public static void AddSingleInfoAccessEmailService(this IServiceCollection services)
        {
            services.AddScoped<IEmailService, EmailService>(serviceProvider =>
            {
                // uses the service provider to get the IMailAccessInfo and IEmailSenderFactory instances
                return new EmailService(serviceProvider.GetRequiredService<IMailAccessInfo>(), serviceProvider.GetRequiredService<IEmailSenderFactory>());
            });
        }

        public static void AddMultipleInfoAccessEmailService(this IServiceCollection services)
        {
            services.AddScoped<IEmailService, EmailService>(serviceProvider =>
            {
                // uses the service provider to get the IMailAccessesInfo and IEmailSenderFactory instances
                return new EmailService(serviceProvider.GetRequiredService<IMailAccessesInfo>(), serviceProvider.GetRequiredService<IEmailSenderFactory>());
            });
        }
    };
}

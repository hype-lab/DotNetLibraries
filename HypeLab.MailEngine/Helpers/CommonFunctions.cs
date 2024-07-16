﻿using HypeLab.MailEngine.Data.Exceptions;
using HypeLab.MailEngine.Data.Models.Impl.Credentials;
using HypeLab.MailEngine.Data.Models;
using HypeLab.MailEngine.Factories;
using HypeLab.MailEngine.SmtpClients;
using HypeLab.MailEngine.Strategies.EmailSender;
using HypeLab.MailEngine.Strategies.EmailSender.Impl;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SendGrid;
using HypeLab.MailEngine.SendGrid;
using HypeLab.MailEngine.Services.Impl;
using HypeLab.MailEngine.Services;
using HypeLab.MailEngine.Factories.Impl;

namespace HypeLab.MailEngine.Helpers
{
    internal static class CommonFunctions
    {
        public static IEmailSender DetermineSenderByClientId(this string clientId, IEmailSenderFactory emailSenderFactory)
        {
            ArgumentException.ThrowIfNullOrEmpty(clientId);

            return emailSenderFactory.CreateEmailSender(clientId);
        }

        public static void AddKeyedScopedMailEngine(this IServiceCollection services, IMailAccessInfo mailAccessInfo, bool isSingleSender = false)
        {
            switch (mailAccessInfo)
            {
                case SmtpAccessInfo smtpAccessInfo:
                    if (isSingleSender)
                        services.AddScoped<IMailAccessInfo, SmtpAccessInfo>(_ => smtpAccessInfo);

                    services.AddKeyedScoped<HypeLabSmtpClient, HypeLabSmtpClient>(serviceKey: smtpAccessInfo.ClientId, implementationFactory: (serviceProvider, _) =>
                    {
                        if (serviceProvider.GetService<IMailAccessesInfo>() is IMailAccessesInfo mailAccessesInfo)
                        {
                            IMailAccessInfo maInfo = mailAccessesInfo.MailAccesses.Single(x => x.ClientId == smtpAccessInfo.ClientId);
                            if (maInfo is SmtpAccessInfo saInfo)
                                return new HypeLabSmtpClient(saInfo);
                        }
                        else if (serviceProvider.GetService<IMailAccessInfo>() is SmtpAccessInfo smtpAccInf)
                        {
                            return new HypeLabSmtpClient(smtpAccInf);
                        }

                        throw new InvalidEmailSenderTypeException("Email sender type not found.");
                    });

                    services.AddKeyedScoped<ISmtpEmailSender, SmtpEmailSender>(serviceKey: smtpAccessInfo.ClientId, implementationFactory: (serviceProvider, _) =>
                    {
                        HypeLabSmtpClient customSmtpClient = serviceProvider.GetRequiredKeyedService<HypeLabSmtpClient>(smtpAccessInfo.ClientId);
                        return new SmtpEmailSender(customSmtpClient);
                    });
                    break;
                case SendGridAccessInfo sgAccessInfo:
                    if (isSingleSender)
                        services.AddScoped<IMailAccessInfo, SendGridAccessInfo>(_ => sgAccessInfo);

                    //services.AddSingleton<ISendGridOptionsProvider, SendGridOptionsProvider>(_ =>
                    //{
                    //    SendGridOptionsProvider provider = new();
                    //    provider.Configure(sgAccessInfo.ClientId, new SendGridClientOptions
                    //    {
                    //        ApiKey = sgAccessInfo.ApiKey,
                    //        RequestHeaders = new Dictionary<string, string>
                    //        {
                    //            { "X-Client-Id", sgAccessInfo.ClientId }
                    //        }
                    //    });
                    //    return provider;
                    //});

                    services.AddKeyedSingleton(serviceKey: sgAccessInfo.ClientId, (_, __) => new SendGridClientOptions
                    {
                        ApiKey = sgAccessInfo.ApiKey,
                        RequestHeaders = new Dictionary<string, string>
                        {
                            { "X-Client-Id", sgAccessInfo.ClientId }
                        }
                    });

                    services.AddHttpClient(sgAccessInfo.ClientId, httpClient => httpClient.Timeout = TimeSpan.FromSeconds(100));

                    services.AddKeyedScoped(serviceKey: sgAccessInfo.ClientId, implementationFactory: (serviceProvider, __) =>
                    {
                        IHttpClientFactory httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
                        HttpClient httpClient = httpClientFactory.CreateClient(sgAccessInfo.ClientId); // crea HttpClient usando il nome univoco

                        SendGridClientOptions options = serviceProvider.GetRequiredKeyedService<SendGridClientOptions>(serviceKey: sgAccessInfo.ClientId);
                        return new HypeLabSendGridClient(httpClient, options);
                    });

                    services.AddKeyedScoped<ISendGridEmailSender, SendGridEmailSender>(serviceKey: sgAccessInfo.ClientId, implementationFactory: (serviceProvider, _) =>
                    {
                        ISendGridClient customSendGridClient = serviceProvider.GetRequiredKeyedService<HypeLabSendGridClient>(sgAccessInfo.ClientId);
                        return new SendGridEmailSender(customSendGridClient);
                    });
                    break;
                default:
                    throw new InvalidEmailSenderTypeException($"The provided email sender type is invalid: {JsonConvert.SerializeObject(mailAccessInfo.GetType())}");
            }
        }

        public static void AddEmailSenderFactory(this IServiceCollection services)
        {
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

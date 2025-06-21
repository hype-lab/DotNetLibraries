using HypeLab.MailEngine.Data.Models.Impl.Credentials;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using HypeLab.MailEngine.Data.Models.Impl.Credentials.ValueTypes;
using HypeLab.MailEngine.Data.Exceptions;

namespace HypeLab.MailEngine.SmtpClients
{
    /// <summary>
    /// Represents an SMTP client configured with advanced options for secure email delivery.
    /// </summary>
    /// <remarks>The <see cref="HypeLabSmtpClient"/> class extends the functionality of the <see
    /// cref="SmtpClient"/> class to provide enhanced configuration capabilities, including support for client
    /// certificates, custom delivery methods, and secure communication settings.</remarks>
    public class HypeLabSmtpClient : SmtpClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HypeLabSmtpClient"/> class using the specified SMTP access
        /// information.
        /// </summary>
        /// <remarks>This constructor configures the SMTP client based on the provided <paramref
        /// name="smtpAccessInfo"/>. It sets properties such as the server host, port, SSL settings, credentials, and
        /// delivery method. If client certificates are provided, they are added to the client for secure communication.
        /// If an error occurs during configuration, an <see cref="SmptClientFailedSettingOptionException"/> is
        /// thrown.</remarks>
        /// <param name="smtpAccessInfo">The SMTP access information used to configure the client. This includes server details, credentials, SSL
        /// settings, delivery options, and optional client certificates.</param>
        /// <exception cref="SmptClientFailedSettingOptionException">Thrown if an error occurs while configuring the SMTP client, such as invalid certificate files or missing
        /// required parameters.</exception>
        public HypeLabSmtpClient(SmtpAccessInfo smtpAccessInfo)
        {
            try
            {
                Host = smtpAccessInfo.Server;
                Port = smtpAccessInfo.Port;
                EnableSsl = smtpAccessInfo.EnableSsl;
                UseDefaultCredentials = smtpAccessInfo.UseDefaultCredentials;
                if (smtpAccessInfo.Timeout.HasValue)
                {
                    Timeout = smtpAccessInfo.Timeout.Value;
                }
                if (!string.IsNullOrEmpty(smtpAccessInfo.PickupDirectoryLocation))
                {
                    PickupDirectoryLocation = smtpAccessInfo.PickupDirectoryLocation;
                }
                if (!string.IsNullOrEmpty(smtpAccessInfo.TargetName))
                {
                    TargetName = smtpAccessInfo.TargetName;
                }
                DeliveryMethod = smtpAccessInfo.DeliveryMethod;
                DeliveryFormat = smtpAccessInfo.DeliveryFormat;
                if (!string.IsNullOrEmpty(smtpAccessInfo.Domain))
                {
                    Credentials = new NetworkCredential(smtpAccessInfo.Email, smtpAccessInfo.Password, smtpAccessInfo.Domain);
                }
                else
                {
                    Credentials = new NetworkCredential(smtpAccessInfo.Email, smtpAccessInfo.Password);
                }
                if (smtpAccessInfo.ClientCertificates?.Count > 0)
                {
                    foreach (SmtpClientCertValue certificate in smtpAccessInfo.ClientCertificates)
                    {
                        X509Certificate2 cert;
                        if (certificate.KeyStorageFlagsEnum.HasValue && !string.IsNullOrEmpty(certificate.Password))
                        {
                            cert = X509Certificate2.CreateFromEncryptedPemFile(certificate.FileName, certificate.Password);
                        }
                        else if (!string.IsNullOrEmpty(certificate.Password))
                        {
                            cert = X509Certificate2.CreateFromEncryptedPemFile(certificate.FileName, certificate.Password);
                        }
                        else
                        {
#if NET9_0_OR_GREATER
                            cert = X509CertificateLoader.LoadCertificateFromFile(certificate.FileName);
#else
                            cert = new X509Certificate2(certificate.FileName);
#endif
                        }

                        ClientCertificates.Add(cert);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new SmptClientFailedSettingOptionException($"Error occurred while creating the SMTP client. {ex.Message} {ex.InnerException?.Message}", ex);
            }
        }

        /// <summary>
        /// Gets the name of the SMTP client.
        /// </summary>
        public static string Name => "Hype-Lab SMTP Client";
    }
}

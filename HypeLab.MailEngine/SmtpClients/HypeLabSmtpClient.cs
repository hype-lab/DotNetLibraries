using HypeLab.MailEngine.Data.Models.Impl.Credentials;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using HypeLab.MailEngine.Data.Models.Impl.Credentials.ValueTypes;
using HypeLab.MailEngine.Data.Exceptions;

namespace HypeLab.MailEngine.SmtpClients
{
    /// <summary>
    /// Represents a custom SMTP client.
    /// </summary>
    public class HypeLabSmtpClient : SmtpClient
    {
        /// <summary>
        /// Constructor for CustomSmtpClient.
        /// </summary>
        /// <param name="smtpAccessInfo"></param>
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

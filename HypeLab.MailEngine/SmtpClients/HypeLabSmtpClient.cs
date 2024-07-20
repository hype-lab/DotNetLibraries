using HypeLab.MailEngine.Data.Models.Impl.Credentials;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using HypeLab.MailEngine.Data.Models.Impl.Credentials.ValueTypes;

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
                foreach (SmtpClientCert certificate in smtpAccessInfo.ClientCertificates)
                {
                    if (certificate.KeyStorageFlagsEnum.HasValue && !string.IsNullOrEmpty(certificate.Password))
                    {
                        ClientCertificates.Add(new X509Certificate(certificate.FileName, certificate.Password, certificate.KeyStorageFlagsEnum.Value));
                    }
                    else if (!string.IsNullOrEmpty(certificate.Password))
                    {
                        ClientCertificates.Add(new X509Certificate(certificate.FileName, certificate.Password));
                    }
                    else
                    {
                        ClientCertificates.Add(new X509Certificate(certificate.FileName));
                    }
                }
            }
        }

        /// <summary>
        /// The name of the SMTP client.
        /// </summary>
        public static string Name => "Hype-Lab SMTP Client";
    }
}

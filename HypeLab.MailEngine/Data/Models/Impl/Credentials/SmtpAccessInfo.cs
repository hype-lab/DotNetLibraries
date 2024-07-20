using HypeLab.MailEngine.Data.Enums;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using HypeLab.MailEngine.Data.Exceptions;

namespace HypeLab.MailEngine.Data.Models.Impl.Credentials
{
    /// <summary>
    /// Represents the access info for the SMTP client.
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="server"></param>
    /// <param name="port"></param>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <param name="enableSsl"></param>
    /// <param name="isDefault"></param>
    /// <param name="domain"></param>
    /// <param name="useDefaultCredentials"></param>
    /// <param name="timeout"></param>
    /// <param name="pickupDirectoryLocation"></param>
    /// <param name="targetName"></param>
    /// <param name="clientCertificates"></param>
    /// <param name="deliveryMethod"></param>
    /// <param name="deliveryFormat"></param>
    public class SmtpAccessInfo(
            string clientId,
            string server,
            int port,
            string email,
            string password,
            bool enableSsl,
            bool? isDefault = null,
            string? domain = null,
            bool? useDefaultCredentials = null,
            int? timeout = null,
            string? pickupDirectoryLocation = null,
            string? targetName = null,
            SmtpClientCert[]? clientCertificates = null,
            SmtpDeliveryMethod deliveryMethod = SmtpDeliveryMethod.Network,
            SmtpDeliveryFormat deliveryFormat = SmtpDeliveryFormat.International
        ) : IMailAccessInfo
    {
        /// <summary>
        /// The type of the email sender.
        /// </summary>
        public EmailSenderType EmailSenderType => EmailSenderType.Smtp;

        /// <summary>
        /// Indicates whether the access info is the default one.
        /// </summary>
        public bool IsDefault { get; } = isDefault ?? true;
        /// <summary>
        /// The client id.
        /// </summary>
        public string ClientId { get; } = clientId;

        /// <summary>
        /// The server.
        /// </summary>
        public string Server { get; } = server;
        /// <summary>
        /// The server port.
        /// </summary>
        public int Port { get; } = port;
        /// <summary>
        /// The account email.
        /// </summary>
        public string Email { get; } = email;
        /// <summary>
        /// The account password.
        /// </summary>
        public string Password { get; } = password;

        /// <summary>
        /// Indicates whether SSL is enabled.
        /// </summary>
        public bool EnableSsl { get; set; } = enableSsl;

        /// <summary>
        /// Indicates whether to use default credentials.
        /// </summary>
        public bool UseDefaultCredentials { get; set; } = useDefaultCredentials ?? false;

        /// <summary>
        /// The NetworkCredentials domain.
        /// </summary>
        public string? Domain { get; set; } = domain;

        /// <summary>
        /// The timeout for the SMTP client.
        /// </summary>
        public int? Timeout { get; set; } = timeout;

        /// <summary>
        /// The pickup directory location.
        /// </summary>
        public string? PickupDirectoryLocation { get; set; } = pickupDirectoryLocation;

        /// <summary>
        /// The target name.
        /// </summary>
        public string? TargetName { get; set; } = targetName;

        /// <summary>
        /// The delivery method.
        /// </summary>
        public SmtpDeliveryMethod DeliveryMethod { get; set; } = deliveryMethod;

        /// <summary>
        /// The delivery format.
        /// </summary>
        public SmtpDeliveryFormat DeliveryFormat { get; set; } = deliveryFormat;

        /// <summary>
        /// The client certificates.
        /// </summary>
        public SmtpClientCert[]? ClientCertificates { get; set; } = clientCertificates;

        /// <summary>
        /// Returns the string representation of the object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"ClientId: {ClientId}, Server: {Server}, Port: {Port}, Email: {Email}, Password: {Password}, EnableSsl: {EnableSsl}, IsDefault: {IsDefault}, Domain: {Domain}, UseDefaultCredentials: {UseDefaultCredentials}, Timeout: {Timeout}, PickupDirectoryLocation: {PickupDirectoryLocation}, TargetName: {TargetName}, DeliveryMethod: {DeliveryMethod}, DeliveryFormat: {DeliveryFormat}, ClientCertificates: {ClientCertificates}";
        }

        /// <summary>
        /// Creates a new instance of the SmtpAccessInfo.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="server"></param>
        /// <param name="port"></param>
        /// <param name="accountEmail"></param>
        /// <param name="password"></param>
        /// <param name="enableSsl"></param>
        /// <param name="isDefault"></param>
        /// <param name="domain"></param>
        /// <param name="useDefaultCredentials"></param>
        /// <param name="timeout"></param>
        /// <param name="pickupDirectoryLocation"></param>
        /// <param name="targetName"></param>
        /// <param name="clientCertificates"></param>
        /// <param name="deliveryMethod"></param>
        /// <param name="deliveryFormat"></param>
        /// <returns></returns>
        public static SmtpAccessInfo Create(
            string clientId,
            string server,
            int port,
            string accountEmail,
            string password,
            bool enableSsl,
            bool isDefault,
            string? domain = null,
            bool? useDefaultCredentials = null,
            int? timeout = null,
            string? pickupDirectoryLocation = null,
            string? targetName = null,
            SmtpClientCert[]? clientCertificates = null,
            SmtpDeliveryMethod deliveryMethod = SmtpDeliveryMethod.Network,
            SmtpDeliveryFormat deliveryFormat = SmtpDeliveryFormat.International)
        {
            return new SmtpAccessInfo(clientId, server, port, accountEmail, password, enableSsl, isDefault, domain, useDefaultCredentials, timeout, pickupDirectoryLocation, targetName, clientCertificates, deliveryMethod, deliveryFormat);
        }
    }

    /// <summary>
    /// Represents the certificate for the SMTP client.
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="password"></param>
    /// <param name="keyStorageFlags"></param>
    public class SmtpClientCert(string fileName, string? password = null, string? keyStorageFlags = null)
    {
        private static X509KeyStorageFlags? ValidateKeyStorageFlags(string? ksf)
        {
            if (!string.IsNullOrEmpty(ksf))
            {
                if (Enum.TryParse(ksf, out X509KeyStorageFlags ksf2))
                    return ksf2;
                else
                    throw new SmtpClientCertException("Invalid key storage flags.");
            }

            return null;
        }

        /// <summary>
        /// The certificate path.
        /// </summary>
        public string FileName { get; set; } = fileName;
        /// <summary>
        /// The certificate password if provided.
        /// </summary>
        public string? Password { get; set; } = password;
        /// <summary>
        /// Set the key storage flags if provided.
        /// </summary>
        public X509KeyStorageFlags? KeyStorageFlags { get; } = ValidateKeyStorageFlags(keyStorageFlags);
    }
}

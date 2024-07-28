using HypeLab.MailEngine.Data.Enums;
using System.Net.Mail;
using HypeLab.MailEngine.Data.Models.Impl.Credentials.ValueTypes;
using Newtonsoft.Json;

namespace HypeLab.MailEngine.Data.Models.Impl.Credentials
{
    /// <summary>
    /// Represents the access info for the SMTP client.
    /// </summary>
    public class SmtpAccessInfo : IMailAccessInfo
    {
        /// <summary>
        /// The type of the email sender.
        /// </summary>
        public EmailSenderType EmailSenderType => EmailSenderType.Smtp;

        /// <summary>
        /// Indicates whether the access info is the default one.
        /// </summary>
        public bool IsDefault { get; }
        /// <summary>
        /// The client id.
        /// </summary>
        public string ClientId { get; }

        /// <summary>
        /// The server.
        /// </summary>
        public string Server { get; }
        /// <summary>
        /// The server port.
        /// </summary>
        public int Port { get; }
        /// <summary>
        /// The account email.
        /// </summary>
        public string Email { get; }
        /// <summary>
        /// The account password.
        /// </summary>
        public string Password { get; }

        /// <summary>
        /// Indicates whether SSL is enabled.
        /// </summary>
        public bool EnableSsl { get; }

        /// <summary>
        /// Indicates whether to use default credentials.
        /// </summary>
        public bool UseDefaultCredentials { get; }

        /// <summary>
        /// The NetworkCredentials domain.
        /// </summary>
        public string? Domain { get; }

        /// <summary>
        /// The timeout for the SMTP client.
        /// </summary>
        public int? Timeout { get; }

        /// <summary>
        /// The pickup directory location.
        /// </summary>
        public string? PickupDirectoryLocation { get; }

        /// <summary>
        /// The target name.
        /// </summary>
        public string? TargetName { get; }

        /// <summary>
        /// The delivery method.
        /// </summary>
        public SmtpDeliveryMethod DeliveryMethod { get; }

        /// <summary>
        /// The delivery format.
        /// </summary>
        public SmtpDeliveryFormat DeliveryFormat { get; }

        /// <summary>
        /// The client certificates.
        /// </summary>
        public HashSet<SmtpClientCertValue>? ClientCertificates { get; }

        /// <summary>
        /// The constructor.
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
        public SmtpAccessInfo(
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
                HashSet<SmtpClientCertValue>? clientCertificates = null,
                SmtpDeliveryMethod deliveryMethod = SmtpDeliveryMethod.Network,
                SmtpDeliveryFormat deliveryFormat = SmtpDeliveryFormat.International
        )
        {
            IsDefault = isDefault ?? true;
            ClientId = clientId;
            Server = server;
            Port = port;
            Email = email;
            Password = password;
            EnableSsl = enableSsl;
            UseDefaultCredentials = useDefaultCredentials ?? false;
            Domain = domain;
            Timeout = timeout;
            PickupDirectoryLocation = pickupDirectoryLocation;
            TargetName = targetName;
            DeliveryMethod = deliveryMethod;
            DeliveryFormat = deliveryFormat;
            ClientCertificates = clientCertificates;
        }

        /// <summary>
        /// Returns the string representation of the object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
            });
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
            bool? isDefault = null,
            string? domain = null,
            bool? useDefaultCredentials = null,
            int? timeout = null,
            string? pickupDirectoryLocation = null,
            string? targetName = null,
            HashSet<SmtpClientCertValue>? clientCertificates = null,
            SmtpDeliveryMethod deliveryMethod = SmtpDeliveryMethod.Network,
            SmtpDeliveryFormat deliveryFormat = SmtpDeliveryFormat.International)
        {
            return new SmtpAccessInfo(clientId, server, port, accountEmail, password, enableSsl, isDefault, domain, useDefaultCredentials, timeout, pickupDirectoryLocation, targetName, clientCertificates, deliveryMethod, deliveryFormat);
        }
    }
}

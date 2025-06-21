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
        /// Gets a value indicating whether this SMTP configuration is the default one.
        /// </summary>
        public bool IsDefault { get; }

        /// <summary>
        /// Gets the unique identifier for the client using this SMTP configuration.
        /// </summary>
        public string ClientId { get; }

        /// <summary>
        /// Gets the hostname or IP address of the SMTP server.
        /// </summary>
        public string Server { get; }

        /// <summary>
        /// Gets the port number used for connecting to the SMTP server.
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// Gets the email address used for authentication and sending messages.
        /// </summary>
        public string Email { get; }

        /// <summary>
        /// Gets the password for the SMTP account.
        /// </summary>
        public string Password { get; }

        /// <summary>
        /// Gets a value indicating whether SSL is enabled for the SMTP connection.
        /// </summary>
        public bool EnableSsl { get; }

        /// <summary>
        /// Gets a value indicating whether default credentials should be used for authentication.
        /// </summary>
        public bool UseDefaultCredentials { get; }

        /// <summary>
        /// Gets the NetworkCredentials domain.
        /// </summary>
        public string? Domain { get; }

        /// <summary>
        /// Gets the timeout duration for SMTP operations, in milliseconds.
        /// </summary>
        public int? Timeout { get; }

        /// <summary>
        /// Gets the directory location where email messages are stored for pickup when using the
        /// </summary>
        public string? PickupDirectoryLocation { get; }

        /// <summary>
        /// Gwets the target name used for authentication when connecting to the SMTP server.
        /// </summary>
        public string? TargetName { get; }

        /// <summary>
        /// Gets the delivery method used to send email messages.
        /// </summary>
        public SmtpDeliveryMethod DeliveryMethod { get; }

        /// <summary>
        /// Gets the delivery format used for sending email messages.
        /// </summary>
        public SmtpDeliveryFormat DeliveryFormat { get; }

        /// <summary>
        /// Gets the collection of client certificates used for authenticating the SMTP client.
        /// </summary>
        /// <remarks>Use this property to retrieve the client certificates that will be used during the
        /// SMTP authentication process. The collection may be empty if no certificates are configured.</remarks>
        public HashSet<SmtpClientCertValue>? ClientCertificates { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SmtpAccessInfo"/> class, which encapsulates configuration details for
        /// accessing an SMTP server.
        /// </summary>
        /// <remarks>This constructor allows for detailed configuration of SMTP access, including authentication, delivery
        /// options, and connection settings.</remarks>
        /// <param name="clientId">A unique identifier for the client using this SMTP configuration. Cannot be null or empty.</param>
        /// <param name="server">The hostname or IP address of the SMTP server. Cannot be null or empty.</param>
        /// <param name="port">The port number to use for connecting to the SMTP server. Must be a positive integer.</param>
        /// <param name="email">The email address used for authentication and sending messages. Cannot be null or empty.</param>
        /// <param name="password">The password associated with the <paramref name="email"/> for authentication. Cannot be null or empty.</param>
        /// <param name="enableSsl">A value indicating whether SSL is enabled for the SMTP connection. Set to <see langword="true"/> to enable SSL;
        /// otherwise, <see langword="false"/>.</param>
        /// <param name="isDefault">Optional. Specifies whether this configuration is the default SMTP configuration. Defaults to <see langword="true"/>
        /// if not provided.</param>
        /// <param name="domain">Optional. The domain name used for authentication. Can be <see langword="null"/> if not required.</param>
        /// <param name="useDefaultCredentials">Optional. Specifies whether default system credentials should be used for authentication. Defaults to <see
        /// langword="false"/> if not provided.</param>
        /// <param name="timeout">Optional. The timeout duration, in milliseconds, for SMTP operations. Can be <see langword="null"/> to use the
        /// default timeout.</param>
        /// <param name="pickupDirectoryLocation">Optional. The directory location where email messages are stored for pickup when using the <see
        /// cref="SmtpDeliveryMethod.SpecifiedPickupDirectory"/> delivery method. Can be <see langword="null"/>.</param>
        /// <param name="targetName">Optional. The target name used for authentication when connecting to the SMTP server. Can be <see langword="null"/>.</param>
        /// <param name="clientCertificates">Optional. A collection of client certificates used for authentication. Can be <see langword="null"/> if no
        /// certificates are required.</param>
        /// <param name="deliveryMethod">Specifies the delivery method for sending email messages. Defaults to <see cref="SmtpDeliveryMethod.Network"/>.</param>
        /// <param name="deliveryFormat">Specifies the delivery format for email messages. Defaults to <see cref="SmtpDeliveryFormat.International"/>.</param>
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
        /// Returns a string representation of the current object in JSON format.
        /// </summary>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
            });
        }

        /// <summary>
        /// Creates a new instance of <see cref="SmtpAccessInfo"/> with the specified SMTP configuration parameters.
        /// </summary>
        /// <param name="clientId">A unique identifier for the client. This value cannot be null or empty.</param>
        /// <param name="server">The hostname or IP address of the SMTP server. This value cannot be null or empty.</param>
        /// <param name="port">The port number to use for the SMTP connection. Must be a positive integer.</param>
        /// <param name="accountEmail">The email address associated with the SMTP account. This value cannot be null or empty.</param>
        /// <param name="password">The password for the SMTP account. This value cannot be null or empty.</param>
        /// <param name="enableSsl">A value indicating whether SSL is enabled for the SMTP connection. <see langword="true"/> to enable SSL;
        /// otherwise, <see langword="false"/>.</param>
        /// <param name="isDefault">Optional. A value indicating whether this configuration is the default SMTP configuration. <see
        /// langword="true"/> if default; otherwise, <see langword="false"/>. If not specified, defaults to <see
        /// langword="null"/>.</param>
        /// <param name="domain">Optional. The domain name to use for authentication. If not specified, defaults to <see langword="null"/>.</param>
        /// <param name="useDefaultCredentials">Optional. A value indicating whether default credentials should be used. <see langword="true"/> to use
        /// default credentials; otherwise, <see langword="false"/>. If not specified, defaults to <see
        /// langword="null"/>.</param>
        /// <param name="timeout">Optional. The timeout duration, in milliseconds, for the SMTP connection. Must be a positive integer if
        /// specified. Defaults to <see langword="null"/>.</param>
        /// <param name="pickupDirectoryLocation">Optional. The file system location where email messages are stored for pickup. If not specified, defaults to
        /// <see langword="null"/>.</param>
        /// <param name="targetName">Optional. The target name used for the SMTP connection. If not specified, defaults to <see
        /// langword="null"/>.</param>
        /// <param name="clientCertificates">Optional. A collection of client certificates to use for authentication. If not specified, defaults to <see
        /// langword="null"/>.</param>
        /// <param name="deliveryMethod">The delivery method for sending emails. Defaults to <see cref="SmtpDeliveryMethod.Network"/>.</param>
        /// <param name="deliveryFormat">The delivery format for sending emails. Defaults to <see cref="SmtpDeliveryFormat.International"/>.</param>
        /// <returns>A new instance of <see cref="SmtpAccessInfo"/> configured with the specified parameters.</returns>
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

using HypeLab.MailEngine.Data.Enums;

namespace HypeLab.MailEngine.Data.Models.Impl.Credentials
{
    /// <summary>
    /// Represents the access info for SMTP.
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="server"></param>
    /// <param name="port"></param>
    /// <param name="accountEmail"></param>
    /// <param name="password"></param>
    /// <param name="enableSsl"></param>
    /// <param name="isDefault"></param>
    public class SmtpAccessInfo(string clientId, string server, int port, string accountEmail, string password, bool enableSsl, bool isDefault) : IMailAccessInfo
    {
        /// <summary>
        /// The type of the email sender.
        /// </summary>
        public EmailSenderType EmailSenderType => EmailSenderType.Smtp;

        /// <summary>
        /// Indicates whether the access info is the default one.
        /// </summary>
        public bool IsDefault { get; } = isDefault;
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
        public string AccountEmail { get; } = accountEmail;
        /// <summary>
        /// The account password.
        /// </summary>
        public string Password { get; } = password;

        /// <summary>
        /// Indicates whether SSL is enabled.
        /// </summary>
        public bool EnableSsl { get; set; } = enableSsl;

        /// <summary>
        /// Returns the string representation of the object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Server: {Server}, Port: {Port}, AccountEmail: {AccountEmail}, Password: {Password}, EnableSsl: {EnableSsl}";
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
        /// <returns></returns>
        public static SmtpAccessInfo Create(string clientId, string server, int port, string accountEmail, string password, bool enableSsl, bool isDefault)
        {
            return new SmtpAccessInfo(clientId, server, port, accountEmail, password, enableSsl, isDefault);
        }
    }
}

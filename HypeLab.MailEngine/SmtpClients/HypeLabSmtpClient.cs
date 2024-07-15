using HypeLab.MailEngine.Data.Models.Impl.Credentials;
using System.Net.Mail;
using System.Net;

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
            EnableSsl = true;
            Credentials = new NetworkCredential(smtpAccessInfo.Email, smtpAccessInfo.Password);
        }

        /// <summary>
        /// The name of the SMTP client.
        /// </summary>
        public static string Name => "Hype-Lab SMTP Client";
    }
}

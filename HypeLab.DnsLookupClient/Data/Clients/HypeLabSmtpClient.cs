using System.Net.Mail;

namespace HypeLab.DnsLookupClient.Data.Clients
{
#pragma warning disable S2094 // Classes should not be empty
    /// <summary>
    /// Represents a specialized SMTP client for sending email messages.
    /// </summary>
    /// <remarks>This class extends the functionality of the <see cref="System.Net.Mail.SmtpClient"/> class.
    /// Use it to send email messages using the Simple Mail Transfer Protocol (SMTP).</remarks>
    public class HypeLabSmtpClient : SmtpClient { }
#pragma warning restore S2094 // Classes should not be empty
}

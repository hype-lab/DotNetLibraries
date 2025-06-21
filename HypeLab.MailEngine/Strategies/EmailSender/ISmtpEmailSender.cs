namespace HypeLab.MailEngine.Strategies.EmailSender
{
    /// <summary>
    /// Defines an email sender that uses the SMTP protocol for sending emails.
    /// </summary>
    /// <remarks>This interface extends <see cref="IEmailSender"/> to provide functionality specific to
    /// SMTP-based email sending. Implementations of this interface should handle the configuration and management of
    /// SMTP settings, such as server address, port, and authentication.</remarks>
    public interface ISmtpEmailSender : IEmailSender
    {
        // Add SMTP-specific methods and properties here
    }
}

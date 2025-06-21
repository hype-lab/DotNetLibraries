namespace HypeLab.MailEngine.Strategies.EmailSender
{
    /// <summary>
    /// Defines an email sender that uses SendGrid for sending emails.
    /// </summary>
    /// <remarks>This interface extends <see cref="IEmailSender"/> to provide functionality specific to
    /// SendGrid. Implementations of this interface should handle email delivery using SendGrid's API.</remarks>
    public interface ISendGridEmailSender : IEmailSender
    {
        // Add SendGrid-specific methods and properties here
    }
}

namespace HypeLab.MailEngine.Data.Enums
{
    /// <summary>
    /// Specifies the type of email sender used for sending emails.
    /// </summary>
    /// <remarks>This enumeration defines the available mechanisms for sending emails, such as SMTP or
    /// SendGrid.</remarks>
    public enum EmailSenderType : byte
    {
        /// <summary>
        /// Represents an unknown state or value.
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Specifies the Simple Mail Transfer Protocol (SMTP) as the communication protocol.
        /// </summary>
        Smtp = 1,
        /// <summary>
        /// Represents the SendGrid email service provider.
        /// </summary>
        SendGrid = 2
    }
}

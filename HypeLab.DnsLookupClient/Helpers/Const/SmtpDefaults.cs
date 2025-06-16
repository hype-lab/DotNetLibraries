namespace HypeLab.DnsLookupClient.Helpers.Const
{
    /// <summary>
    /// Provides default SMTP (Simple Mail Transfer Protocol) command strings used in email communication.
    /// </summary>
    /// <remarks>This class contains constants representing common SMTP commands, such as HELO, MAIL FROM,
    /// RCPT TO, and QUIT. These commands are typically used when interacting with an SMTP server to send email
    /// messages.</remarks>
    public static class SmtpDefaults
    {
        /// <summary>
        /// Represents the SMTP HELO command used to initiate a conversation with an SMTP server.
        /// </summary>
        /// <remarks>The HELO command is typically used in the initial handshake of the SMTP protocol to
        /// identify the client to the server.</remarks>
        public const string SmtpHeloCommand = "HELO";
        /// <summary>
        /// Represents the SMTP "MAIL FROM" command format string used to specify the sender's email address.
        /// </summary>
        /// <remarks>The format string includes a placeholder (<c>{0}</c>) for the sender's email address.
        /// Use <see cref="string.Format(string, object)"/> to replace the placeholder with the actual email
        /// address.</remarks>
        public const string SmtpMailFromCommand = "MAIL FROM:<{0}>";
        /// <summary>
        /// Represents the SMTP "RCPT TO" command format string used to specify the recipient of an email.
        /// </summary>
        /// <remarks>The format string includes a placeholder for the recipient's email address, which
        /// should be provided when constructing the command.</remarks>
        public const string SmtpRcptToCommand = "RCPT TO:<{0}>";
        /// <summary>
        /// Represents the SMTP command used to terminate the connection with the server.
        /// </summary>
        /// <remarks>The "QUIT" command is sent by the client to inform the SMTP server that the session
        /// is ending. After receiving this command, the server typically closes the connection.</remarks>
        public const string SmtpQuitCommand = "QUIT";
    }
}

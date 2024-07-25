namespace HypeLab.MailEngine.Data.Models.Impl.Base
{
    /// <summary>
    /// Represents the generic mail message.
    /// </summary>
    public interface IMailMessage
    {
        /// <summary>
        /// The email subject.
        /// </summary>
        public string EmailSubject { get; set; }

        /// <summary>
        /// The email from.
        /// </summary>
        public string EmailFrom { get; set; }

        /// <summary>
        /// The html message.
        /// </summary>
        public string HtmlMessage { get; set; }

        /// <summary>
        /// The plain text content.
        /// </summary>
        public string? PlainTextContent { get; set; }

        /// <summary>
        /// The email to name.
        /// </summary>
        public string? EmailToName { get; set; }

        /// <summary>
        /// The email from name.
        /// </summary>
        public string? EmailFromName { get; set; }

        /// <summary>
        /// The ccs.
        /// </summary>
        public IEmailAddressInfo[]? Ccs { get; set; }
    }
}

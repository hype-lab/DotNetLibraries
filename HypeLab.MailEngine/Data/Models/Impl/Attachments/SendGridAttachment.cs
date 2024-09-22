namespace HypeLab.MailEngine.Data.Models.Impl.Attachments
{
    /// <summary>
    /// Represents a SendGrid attachment.
    /// </summary>
    /// <remarks>
    /// Constructor.
    /// </remarks>
    /// <param name="name"></param>
    /// <param name="type"></param>
    /// <param name="content"></param>
    public class SendGridAttachment(string name, string type, byte[] content) : IAttachment
    {
        /// <summary>
        /// The name of the attachment.
        /// </summary>
        public string Name { get; set; } = name;
        /// <summary>
        /// The content id of the attachment.
        /// </summary>
        public string? ContentId { get; set; }
        /// <summary>
        /// The type of the attachment.
        /// </summary>
        public string Type { get; set; } = type;
        /// <summary>
        /// The content of the attachment.
        /// </summary>
        public byte[] Content { get; set; } = content;
        /// <summary>
        /// The disposition of the attachment.
        /// </summary>
        public string? Disposition { get; set; }
    }
}

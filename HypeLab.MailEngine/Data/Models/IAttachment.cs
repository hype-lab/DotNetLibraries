namespace HypeLab.MailEngine.Data.Models
{
    /// <summary>
    /// Represents an attachment.
    /// </summary>
    public interface IAttachment
    {
        /// <summary>
        /// The name of the attachment.
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// The type of the attachment.
        /// </summary>
        string? ContentId { get; set; }
    }
}

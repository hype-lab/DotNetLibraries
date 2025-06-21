namespace HypeLab.MailEngine.Data.Models
{
    /// <summary>
    /// Represents an attachment with a name and an optional content identifier.
    /// </summary>
    /// <remarks>This interface is typically used to define metadata for attachments, such as files or resources, that
    /// can be associated with other entities or operations.</remarks>
    public interface IAttachment
    {
        /// <summary>
        /// Gets or sets the name of the attachment.
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Gets or sets the content identifier of the attachment, which is used to reference the attachment in
        /// </summary>
        string? ContentId { get; set; }
    }
}

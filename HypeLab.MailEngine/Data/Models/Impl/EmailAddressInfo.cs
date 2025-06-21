namespace HypeLab.MailEngine.Data.Models.Impl
{
    /// <summary>
    /// Represents information about an email address, including its associated name and recipient type.
    /// </summary>
    /// <remarks>This class provides details about an email address, such as whether it is a primary
    /// recipient, a carbon copy (CC) recipient, or a blind carbon copy (BCC) recipient.</remarks>
    public class EmailAddressInfo : IEmailAddressInfo
    {
        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Gets or sets the name associated with the email address.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates if the email address is a recipient.
        /// </summary>
        public bool IsTo { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates if the email address is a carbon copy recipient.
        /// </summary>
        public bool IsCc { get; set; }
        /// <summary>
        /// Gets or sets a value that indicates if the email address is a blind carbon copy recipient.
        /// </summary>
        public bool IsBcc { get; set; }
    }
}

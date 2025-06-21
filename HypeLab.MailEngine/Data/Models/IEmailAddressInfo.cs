namespace HypeLab.MailEngine.Data.Models
{
    /// <summary>
    /// Represents information about an email address, including its associated name and recipient type.
    /// </summary>
    /// <remarks>This interface provides properties to describe an email address and its role in email
    /// communication. It can be used to specify whether the email address is a primary recipient, a carbon copy (CC)
    /// recipient, or a blind carbon copy (BCC) recipient.</remarks>
    public interface IEmailAddressInfo
    {
        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        string Email { get; set; }

        /// <summary>
        /// Gets or sets the name associated with the email address.
        /// </summary>
        string? Name { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates if the email address is a recipient.
        /// </summary>
        bool IsTo { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates if the email address is a carbon copy recipient.
        /// </summary>
        bool IsCc { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates if the email address is a blind carbon copy recipient.
        /// </summary>
        bool IsBcc { get; set; }
    }
}

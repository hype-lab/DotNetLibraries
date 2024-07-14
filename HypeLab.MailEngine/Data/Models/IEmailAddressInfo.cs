namespace HypeLab.MailEngine.Data.Models
{
    /// <summary>
    /// Represents an email address.
    /// </summary>
    public interface IEmailAddressInfo
    {
        /// <summary>
        /// The email address.
        /// </summary>
        string Email { get; set; }
        /// <summary>
        /// The name of the email address.
        /// </summary>
        string? Name { get; set; }

        /// <summary>
        /// Indicates if the email address is a recipient.
        /// </summary>
        bool IsTo { get; set; }
        /// <summary>
        /// Indicates if the email address is a carbon copy recipient.
        /// </summary>
        bool IsCc { get; set; }
        /// <summary>
        /// Indicates if the email address is a blind carbon copy recipient.
        /// </summary>
        bool IsBcc { get; set; }
    }
}

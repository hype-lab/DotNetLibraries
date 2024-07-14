namespace HypeLab.MailEngine.Data.Models.Impl
{
    /// <summary>
    /// Represents the email address info.
    /// </summary>
    public class EmailAddressInfo : IEmailAddressInfo
    {
        /// <summary>
        /// The email.
        /// </summary>
        public required string Email { get; set; }
        /// <summary>
        /// The email name.
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Indicates if the email address is a recipient.
        /// </summary>
        public bool IsTo { get; set; }
        /// <summary>
        /// Indicates if the email address is a carbon copy recipient.
        /// </summary>
        public bool IsCc { get; set; }
        /// <summary>
        /// Indicates if the email address is a blind carbon copy recipient.
        /// </summary>
        public bool IsBcc { get; set; }
    }
}

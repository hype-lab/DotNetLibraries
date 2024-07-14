namespace HypeLab.MailEngine.Data.Enums
{
    /// <summary>
    /// Email sender type
    /// </summary>
    public enum EmailSenderType : sbyte
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Smtp
        /// </summary>
        Smtp = 1,
        /// <summary>
        /// SendGrid
        /// </summary>
        SendGrid = 2
    }
}

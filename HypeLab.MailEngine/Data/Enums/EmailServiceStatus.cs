namespace HypeLab.MailEngine.Data.Enums
{
    /// <summary>
    /// Email service status
    /// </summary>
    public enum EmailServiceStatus : sbyte
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Error
        /// </summary>
        Error = 1,
        /// <summary>
        /// Warning
        /// </summary>
        Warning = 2,
        /// <summary>
        /// Success
        /// </summary>
        Success = 3
    }
}

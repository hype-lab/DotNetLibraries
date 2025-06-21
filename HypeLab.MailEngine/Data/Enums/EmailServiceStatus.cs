namespace HypeLab.MailEngine.Data.Enums
{
    /// <summary>
    /// Represents the status of an email service operation.
    /// </summary>
    /// <remarks>This enumeration is used to indicate the outcome of an email service operation,  such as
    /// sending an email or processing a request. The status can be one of the following: <list type="bullet"> <item>
    /// <term><see cref="Unknown"/></term> <description>The status of the operation is not known.</description> </item>
    /// <item> <term><see cref="Error"/></term> <description>The operation encountered an error and did not complete
    /// successfully.</description> </item> <item> <term><see cref="Warning"/></term> <description>The operation
    /// completed with warnings, indicating potential issues.</description> </item> <item> <term><see
    /// cref="Success"/></term> <description>The operation completed successfully without any issues.</description>
    /// </item> </list></remarks>
    public enum EmailServiceStatus : byte
    {
        /// <summary>
        /// Represents an unknown state or value.
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Represents an error state in the application.
        /// </summary>
        Error = 1,
        /// <summary>
        /// Represents a warning-level severity for logging or diagnostic messages.
        /// </summary>
        Warning = 2,
        /// <summary>
        /// Represents a successful operation or outcome.
        /// </summary>
        Success = 3
    }
}

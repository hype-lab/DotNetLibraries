using HypeLab.MailEngine.Helpers.Const;
using System.Diagnostics;

namespace HypeLab.MailEngine.Data.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when an SMTP client fails to set a required option.
    /// </summary>
    /// <remarks>This exception is typically thrown when an SMTP client encounters an error while attempting
    /// to configure a required setting, such as authentication credentials, server options, or other configuration
    /// parameters.</remarks>
    [DebuggerDisplay(ExceptionDefaults.SmptClientFailedSettingOption.DebuggerDisplay)]
    public class SmptClientFailedSettingOptionException : Exception
    {
        /// <summary>
        /// Creates a new instance of the <see cref="SmptClientFailedSettingOptionException"/> class with a default message.
        /// </summary>
        public SmptClientFailedSettingOptionException()
            : base(ExceptionDefaults.SmptClientFailedSettingOption.DefaultMessage) { }

        /// <summary>
        /// Creates a new instance of the <see cref="SmptClientFailedSettingOptionException"/> class with a specified message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public SmptClientFailedSettingOptionException(string? message)
            : base(message ?? ExceptionDefaults.SmptClientFailedSettingOption.DefaultMessage) { }

        /// <summary>
        /// Creates a new instance of the <see cref="SmptClientFailedSettingOptionException"/> class with a specified message and an inner exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public SmptClientFailedSettingOptionException(string? message, Exception innerException)
            : base(message ?? ExceptionDefaults.SmptClientFailedSettingOption.DefaultMessage, innerException) { }
    }
}

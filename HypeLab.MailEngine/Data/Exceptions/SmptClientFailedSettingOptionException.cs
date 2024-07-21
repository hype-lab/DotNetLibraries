using HypeLab.MailEngine.Helpers.Const;
using System.Diagnostics;

namespace HypeLab.MailEngine.Data.Exceptions
{
    /// <summary>
    /// Exception class for when the SMTP client fails to set an option
    /// </summary>
    [DebuggerDisplay(ExceptionDefaults.SmptClientFailedSettingOption.DebuggerDisplay)]
    public class SmptClientFailedSettingOptionException : Exception
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public SmptClientFailedSettingOptionException()
            : base(ExceptionDefaults.SmptClientFailedSettingOption.DefaultMessage) { }

        /// <summary>
        /// Constructor with message
        /// </summary>
        public SmptClientFailedSettingOptionException(string? message)
            : base(message ?? ExceptionDefaults.SmptClientFailedSettingOption.DefaultMessage) { }

        /// <summary>
        /// Constructor with message
        /// </summary>
        public SmptClientFailedSettingOptionException(string? message, Exception innerException)
            : base(message ?? ExceptionDefaults.SmptClientFailedSettingOption.DefaultMessage, innerException) { }
    }
}

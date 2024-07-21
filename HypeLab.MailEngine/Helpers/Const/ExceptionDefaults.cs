namespace HypeLab.MailEngine.Helpers.Const
{
    /// <summary>
    /// Default exception properties.
    /// </summary>
    public static class ExceptionDefaults
    {
        /// <summary>
        /// Default exception properties for the <see cref="Data.Exceptions.MultipleDefaultEmailSendersFoundException"/>.
        /// </summary>
        public static class MultipleDefaultEmailSendersFound
        {
            /// <summary>
            /// The debugger display string to set.
            /// </summary>
            public const string DebuggerDisplay = "MultipleDefaultEmailSendersFoundException: {Message}";
            /// <summary>
            /// The default exception message.
            /// </summary>
            public const string DefaultMessage = "Multiple default email senders found. One default email sender must be set";
        }

        /// <summary>
        /// Default exception properties for the <see cref="Data.Exceptions.DefaultEmailSenderNotFoundException"/>.
        /// </summary>
        public static class DefaultEmailSenderNotFound
        {
            /// <summary>
            /// The debugger display string to set.
            /// </summary>
            public const string DebuggerDisplay = "DefaultEmailSenderNotFoundException: {Message}";
            /// <summary>
            /// The default exception message.
            /// </summary>
            public const string DefaultMessage = "Default email sender not found.";
        }

        /// <summary>
        /// Default exception properties for the <see cref="Data.Exceptions.InvalidEmailSenderTypeException"/>.
        /// </summary>
        public static class InvalidEmailSenderType
        {
            /// <summary>
            /// The debugger display string to set.
            /// </summary>
            public const string DebuggerDisplay = "InvalidEmailSenderTypeException: {Message}";
            /// <summary>
            /// The default exception message.
            /// </summary>
            public const string DefaultMessage = "Invalid email sender type.";
        }

        /// <summary>
        /// Default exception properties for the <see cref="Data.Exceptions.MailAccessInfoClientIdNullException"/>.
        /// </summary>
        public static class MailAccessInfoClientIdNull
        {
            /// <summary>
            /// The debugger display string to set.
            /// </summary>
            public const string DebuggerDisplay = "MailAccessInfoClientIdNullException: {Message}";
            /// <summary>
            /// The default exception message.
            /// </summary>
            public const string DefaultMessage = "ClientId cannot be null or empty.";
        }

        /// <summary>
        /// Default exception properties for the <see cref="Data.Exceptions.DuplicateClientIdNamesException"/>.
        /// </summary>
        public static class DuplicateClientIdNames
        {
            /// <summary>
            /// The debugger display string to set.
            /// </summary>
            public const string DebuggerDisplay = "DuplicateClientIdNamesException: {Message}";
            /// <summary>
            /// The default exception message.
            /// </summary>
            public const string DefaultMessage = "Duplicate client id names found.";
        }

        /// <summary>
        /// Default exception properties for the <see cref="Data.Exceptions.SmtpClientCertException"/>.
        /// </summary>
        public static class SmtpClientCert
        {
            /// <summary>
            /// The debugger display string to set.
            /// </summary>
            public const string DebuggerDisplay = "SmtpClientCertException: {Message}";
            /// <summary>
            /// The default exception message.
            /// </summary>
            public const string DefaultMessage = "Smtp client certificate not found.";
        }

        /// <summary>
        /// Default exception properties for the <see cref="Data.Exceptions.SmptClientFailedSettingOptionException"/>.
        /// </summary>
        public static class SmptClientFailedSettingOption
        {
            /// <summary>
            /// The debugger display string to set.
            /// </summary>
            public const string DebuggerDisplay = "SmptClientFailedSettingOptionException: {Message}";
            /// <summary>
            /// The default exception message.
            /// </summary>
            public const string DefaultMessage = "Failed to set SMTP client option.";
        }
    }
}

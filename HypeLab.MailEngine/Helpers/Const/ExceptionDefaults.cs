namespace HypeLab.MailEngine.Helpers.Const
{
    /// <summary>
    /// Default exception messages.
    /// </summary>
    public static class ExceptionDefaults
    {
        /// <summary>
        /// Default exception messages for the <see cref="Data.Models.IMailAccessInfo"/>.
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
            public const string DefaultMessage = "Multiple default email senders found.";
        }

        /// <summary>
        /// Default exception messages for the <see cref="Data.Models.IMailAccessInfo"/>.
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
        /// Default exception messages for the <see cref="Data.Models.IMailAccessInfo"/>.
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
        /// Default exception messages for the <see cref="Data.Models.IMailAccessInfo"/>.
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
            public const string DefaultMessage = "ClientId cannot be null.";
        }

        /// <summary>
        /// Default exception messages for the <see cref="Data.Models.IMailAccessInfo"/>.
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
    }
}

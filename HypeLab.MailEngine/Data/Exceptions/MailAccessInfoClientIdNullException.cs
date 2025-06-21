using HypeLab.MailEngine.Data.Models;
using HypeLab.MailEngine.Helpers.Const;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace HypeLab.MailEngine.Data.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when the client ID of a mail access information object is null or empty.
    /// </summary>
    [DebuggerDisplay(ExceptionDefaults.MailAccessInfoClientIdNull.DebuggerDisplay)]
    public class MailAccessInfoClientIdNullException : Exception
    {
        /// <summary>
        /// Creates a new instance of the <see cref="MailAccessInfoClientIdNullException"/> class with a default message.
        /// </summary>
        public MailAccessInfoClientIdNullException()
            : base(ExceptionDefaults.MailAccessInfoClientIdNull.DefaultMessage) { }

        /// <summary>
        /// Creates a new instance of the <see cref="MailAccessInfoClientIdNullException"/> class with a specified message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public MailAccessInfoClientIdNullException(string? message)
            : base(message ?? ExceptionDefaults.MailAccessInfoClientIdNull.DefaultMessage) { }

        /// <summary>
        /// Creates a new instance of the <see cref="MailAccessInfoClientIdNullException"/> class with a specified message and an inner exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public MailAccessInfoClientIdNullException(string? message, Exception innerException)
            : base(message ?? ExceptionDefaults.MailAccessInfoClientIdNull.DefaultMessage, innerException) { }

        /// <summary>
        /// Validates that the <see cref="IMailAccessInfo.ClientId"/> property of each item in the specified collection
        /// is not null or empty. Throws an exception if any <see cref="IMailAccessInfo.ClientId"/> is invalid.
        /// </summary>
        /// <remarks>This method iterates through the provided collection and checks the <see
        /// cref="IMailAccessInfo.ClientId"/> property of each item. If any <see cref="IMailAccessInfo.ClientId"/> is
        /// null or empty, an exception is thrown.</remarks>
        /// <param name="mailAccessInfos">A collection of <see cref="IMailAccessInfo"/> objects to validate. Cannot be null.</param>
        /// <param name="message">An optional custom error message to include in the exception. If not provided, a default message is used.</param>
        public static void ThrowIfClientIdNullOrEmpty(ICollection<IMailAccessInfo> mailAccessInfos, string? message = null)
        {
#pragma warning disable S3267 // Loops should be simplified with "LINQ" expressions
            foreach (IMailAccessInfo mailAccessInfo in mailAccessInfos)
            {
                if (string.IsNullOrEmpty(mailAccessInfo.ClientId))
                    Throw(message ?? ExceptionDefaults.MailAccessInfoClientIdNull.DefaultMessage);
            }
#pragma warning restore S3267 // Loops should be simplified with "LINQ" expressions
        }

        /// <summary>
        /// Validates that the <see cref="IMailAccessInfo.ClientId"/> property is not null or empty.
        /// </summary>
        /// <param name="mailAccessInfo">The mail access information object containing the <see cref="IMailAccessInfo.ClientId"/> to validate.</param>
        /// <param name="message">An optional custom error message to include in the exception if validation fails.  If not provided, a
        /// default error message will be used.</param>
        public static void ThrowIfClientIdNullOrEmpty(IMailAccessInfo mailAccessInfo, string? message = null)
        {
            if (string.IsNullOrEmpty(mailAccessInfo.ClientId))
                Throw(message ?? ExceptionDefaults.MailAccessInfoClientIdNull.DefaultMessage);
        }

        [DoesNotReturn]
        private static void Throw(string message)
            => throw new MailAccessInfoClientIdNullException(message);
    }
}

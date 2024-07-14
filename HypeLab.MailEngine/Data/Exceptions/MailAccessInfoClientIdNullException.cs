using HypeLab.MailEngine.Data.Models;
using HypeLab.MailEngine.Helpers.Const;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace HypeLab.MailEngine.Data.Exceptions
{
    /// <summary>
    /// Exception class for when the client id is null or empty
    /// </summary>
    [DebuggerDisplay(ExceptionDefaults.MailAccessInfoClientIdNull.DebuggerDisplay)]
    public class MailAccessInfoClientIdNullException : Exception
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public MailAccessInfoClientIdNullException()
            : base(ExceptionDefaults.MailAccessInfoClientIdNull.DefaultMessage) { }

        /// <summary>
        /// Constructor with message
        /// </summary>
        public MailAccessInfoClientIdNullException(string message)
            : base(message) { }

        /// <summary>
        /// Constructor with message and inner exception
        /// </summary>
        public MailAccessInfoClientIdNullException(string message, Exception innerException)
            : base(message, innerException) { }

        /// <summary>
        /// Throws an exception if the client id is null or empty
        /// </summary>
        /// <param name="message">Optional error message</param>
        /// <param name="mailAccessInfos">Arguments to check</param>
        public static void ThrowIfClientIdNullOrEmpty(string? message, params IMailAccessInfo[] mailAccessInfos)
        {
#pragma warning disable S3267 // Loops should be simplified with "LINQ" expressions
            foreach (IMailAccessInfo mailAccessInfo in mailAccessInfos)
            {
                if (string.IsNullOrEmpty(mailAccessInfo.ClientId))
                    Throw(message ?? ExceptionDefaults.MailAccessInfoClientIdNull.DefaultMessage);
            }
#pragma warning restore S3267 // Loops should be simplified with "LINQ" expressions
        }

        [DoesNotReturn]
        private static void Throw(string message)
            => throw new MailAccessInfoClientIdNullException(message);
    }
}

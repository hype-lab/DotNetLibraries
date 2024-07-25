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
        /// <param name="message"></param>
        public MailAccessInfoClientIdNullException(string? message)
            : base(message ?? ExceptionDefaults.MailAccessInfoClientIdNull.DefaultMessage) { }

        /// <summary>
        /// Constructor with message and inner exception
        /// </summary>
        public MailAccessInfoClientIdNullException(string? message, Exception innerException)
            : base(message ?? ExceptionDefaults.MailAccessInfoClientIdNull.DefaultMessage, innerException) { }

        /// <summary>
        /// Throws an exception if the client id is null or empty
        /// </summary>
        /// <param name="mailAccessInfos"></param>
        /// <param name="message"></param>
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
        /// Throws an exception if the client id is null or empty
        /// </summary>
        /// <param name="mailAccessInfo"></param>
        /// <param name="message"></param>
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

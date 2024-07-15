using HypeLab.MailEngine.Data.Models;
using HypeLab.MailEngine.Helpers.Const;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace HypeLab.MailEngine.Data.Exceptions
{
    /// <summary>
    /// Exception class for when there are duplicate client id names
    /// </summary>
    [DebuggerDisplay(ExceptionDefaults.DuplicateClientIdNames.DebuggerDisplay)]
    public class DuplicateClientIdNamesException : Exception
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public DuplicateClientIdNamesException()
            : base(ExceptionDefaults.DuplicateClientIdNames.DefaultMessage) { }

        /// <summary>
        /// Constructor with message
        /// </summary>
        public DuplicateClientIdNamesException(string? message)
            : base(message ?? ExceptionDefaults.DuplicateClientIdNames.DefaultMessage) { }

        /// <summary>
        /// Constructor with message and inner exception
        /// </summary>
        public DuplicateClientIdNamesException(string? message, Exception innerException)
            : base(message ?? ExceptionDefaults.DuplicateClientIdNames.DefaultMessage, innerException) { }

        /// <summary>
        /// Throws an exception if there are duplicate client id names
        /// </summary>
        /// <param name="arguments">Arguments to check</param>
        /// <param name="message">Optional error message</param>
        public static void ThrowIfDuplicateClientIdNames(IEnumerable<IMailAccessInfo> arguments, string? message = null)
        {
            List<string> clientIds = arguments.Select(x => x.ClientId).ToList();

            if (clientIds.GroupBy(x => x).Where(g => g.Count() > 1).Select(y => y.Key).Any())
                Throw(message ?? ExceptionDefaults.DuplicateClientIdNames.DefaultMessage);
        }

        [DoesNotReturn]
        private static void Throw(string message)
            => throw new DuplicateClientIdNamesException(message);
    }
}

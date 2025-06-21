using HypeLab.MailEngine.Data.Models;
using HypeLab.MailEngine.Helpers.Const;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace HypeLab.MailEngine.Data.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when duplicate client ID names are detected.
    /// </summary>
    [DebuggerDisplay(ExceptionDefaults.DuplicateClientIdNames.DebuggerDisplay)]
    public class DuplicateClientIdNamesException : Exception
    {
        /// <summary>
        /// Creates a new instance of the <see cref="DuplicateClientIdNamesException"/> class with a default message.
        /// </summary>
        public DuplicateClientIdNamesException()
            : base(ExceptionDefaults.DuplicateClientIdNames.DefaultMessage) { }

        /// <summary>
        /// Creates a new instance of the <see cref="DuplicateClientIdNamesException"/> class with a specified message.
        /// </summary>
        public DuplicateClientIdNamesException(string? message)
            : base(message ?? ExceptionDefaults.DuplicateClientIdNames.DefaultMessage) { }

        /// <summary>
        /// Creates a new instance of the <see cref="DuplicateClientIdNamesException"/> class with a specified message and an inner exception.
        /// </summary>
        public DuplicateClientIdNamesException(string? message, Exception innerException)
            : base(message ?? ExceptionDefaults.DuplicateClientIdNames.DefaultMessage, innerException) { }

        /// <summary>
        /// Throws an exception if the provided collection contains duplicate client ID names.
        /// </summary>
        /// <remarks>This method checks for duplicate client IDs in the provided collection and throws an
        /// exception  if any duplicates are detected. The caller can optionally specify a custom error
        /// message.</remarks>
        /// <param name="arguments">A collection of <see cref="IMailAccessInfo"/> objects, each containing a client ID to be validated.</param>
        /// <param name="message">An optional custom error message to include in the exception.  If not provided, a default message will be
        /// used.</param>
        public static void ThrowIfDuplicateClientIdNames(IEnumerable<IMailAccessInfo> arguments, string? message = null)
        {
            List<string> clientIds = [.. arguments.Select(x => x.ClientId)];

            if (clientIds.GroupBy(x => x).Where(g => g.Count() > 1).Select(y => y.Key).Any())
                Throw(message ?? ExceptionDefaults.DuplicateClientIdNames.DefaultMessage);
        }

        [DoesNotReturn]
        private static void Throw(string message)
            => throw new DuplicateClientIdNamesException(message);
    }
}

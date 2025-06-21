using HypeLab.MailEngine.Helpers.Const;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace HypeLab.MailEngine.Data.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when reliability properties are invalid.
    /// </summary>
    [DebuggerDisplay(ExceptionDefaults.InvalidReliabilityProperties.DebuggerDisplay)]
    public class InvalidReliabilityPropertiesException : Exception
    {
        /// <summary>
        /// Creates a new instance of the <see cref="InvalidReliabilityPropertiesException"/> class with a default message.
        /// </summary>
        public InvalidReliabilityPropertiesException()
            : base(ExceptionDefaults.InvalidReliabilityProperties.DefaultMessage) { }

        /// <summary>
        /// Creates a new instance of the <see cref="InvalidReliabilityPropertiesException"/> class with a specified message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InvalidReliabilityPropertiesException(string? message)
            : base(message ?? ExceptionDefaults.InvalidReliabilityProperties.DefaultMessage) { }

        /// <summary>
        /// Creates a new instance of the <see cref="InvalidReliabilityPropertiesException"/> class with a specified message and an inner exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public InvalidReliabilityPropertiesException(string? message, Exception innerException)
            : base(message ?? ExceptionDefaults.InvalidReliabilityProperties.DefaultMessage, innerException) { }

        /// <summary>
        /// Validates the reliability properties and throws an exception if any of them are invalid.
        /// </summary>
        /// <param name="maximumNumberOfRetries">The maximum number of retry attempts. Must not be null.</param>
        /// <param name="minimumBackOffInSeconds">The minimum backoff duration, in seconds, between retries. Must not be null.</param>
        /// <param name="deltaBackOffInSeconds">The incremental backoff duration, in seconds, applied to each retry. Must not be null.</param>
        /// <param name="maximumBackOffInSeconds">The maximum backoff duration, in seconds, allowed between retries. Must not be null.</param>
        /// <param name="message">An optional custom message for the exception. If not provided, a default message will be used.</param>
        public static void ThrowIfReliabilityPropertiesAreInvalid(int? maximumNumberOfRetries, int? minimumBackOffInSeconds, int? deltaBackOffInSeconds, int? maximumBackOffInSeconds, string? message = null)
        {
            if (!maximumNumberOfRetries.HasValue || !minimumBackOffInSeconds.HasValue || !deltaBackOffInSeconds.HasValue || !maximumBackOffInSeconds.HasValue)
                Throw(message ?? ExceptionDefaults.InvalidReliabilityProperties.DefaultMessage);
        }

        [DoesNotReturn]
        private static void Throw(string message)
            => throw new InvalidReliabilityPropertiesException(message);
    }
}

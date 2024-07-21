using HypeLab.MailEngine.Helpers.Const;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypeLab.MailEngine.Data.Exceptions
{
    /// <summary>
    /// Exception class for when the reliability properties are invalid
    /// </summary>
    [DebuggerDisplay(ExceptionDefaults.InvalidReliabilityProperties.DebuggerDisplay)]
    public class InvalidReliabilityPropertiesException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public InvalidReliabilityPropertiesException()
            : base(ExceptionDefaults.InvalidReliabilityProperties.DefaultMessage) { } 

        /// <summary>
        /// Constructor with message
        /// </summary>
        /// <param name="message"></param>
        public InvalidReliabilityPropertiesException(string? message)
            : base(message ?? ExceptionDefaults.InvalidReliabilityProperties.DefaultMessage) { }

        /// <summary>
        /// Constructor with message and inner exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public InvalidReliabilityPropertiesException(string? message, Exception innerException)
            : base(message ?? ExceptionDefaults.InvalidReliabilityProperties.DefaultMessage, innerException) { }

        /// <summary>
        /// Throws an exception if the reliability properties are invalid.
        /// </summary>
        /// <param name="maximumNumberOfRetries"></param>
        /// <param name="minimumBackOffInSeconds"></param>
        /// <param name="deltaBackOffInSeconds"></param>
        /// <param name="maximumBackOffInSeconds"></param>
        /// <param name="message"></param>
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

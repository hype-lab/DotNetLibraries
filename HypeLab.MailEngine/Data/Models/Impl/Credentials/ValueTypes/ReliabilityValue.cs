using HypeLab.MailEngine.Data.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace HypeLab.MailEngine.Data.Models.Impl.Credentials.ValueTypes
{
    /// <summary>
    /// Represents a set of reliability parameters used to configure retry and backoff mechanisms for operations.
    /// </summary>
    public struct ReliabilityValue
    {
        /// <summary>
        /// Validates that all required reliability properties are set.
        /// </summary>
        /// <remarks>This method ensures that the properties <see cref="MaximumBackOffInSeconds"/>, <see
        /// cref="MaximumNumberOfRetries"/>, <see cref="DeltaBackOffInSeconds"/>, and <see
        /// cref="MinimumBackOffInSeconds"/> are assigned valid values. If any of these properties are not set, an
        /// exception is thrown.</remarks>
        /// <exception cref="InvalidReliabilityPropertiesException">Thrown if one or more required reliability properties are not provided.</exception>
        [MemberNotNull(nameof(MaximumBackOffInSeconds), nameof(MaximumNumberOfRetries), nameof(DeltaBackOffInSeconds), nameof(MinimumBackOffInSeconds))]
        public readonly void Validate()
        {
            if (!MaximumNumberOfRetries.HasValue || !MinimumBackOffInSeconds.HasValue || !DeltaBackOffInSeconds.HasValue || !MaximumBackOffInSeconds.HasValue)
                throw new InvalidReliabilityPropertiesException("All reliability properties must be provided.");
        }

        /// <summary>
        /// Gets or sets the maximum number of retry attempts for a failed operation.
        /// </summary>
        public int? MaximumNumberOfRetries { get; set; }

        /// <summary>
        /// Gets or sets the minimum backoff time, in seconds, for retry attempts.
        /// </summary>
        public int? MinimumBackOffInSeconds { get; set; }

        /// <summary>
        /// Gets or sets the incremental backoff time, in seconds, used for retry operations.
        /// </summary>
        public int? DeltaBackOffInSeconds { get; set; }

        /// <summary>
        /// Gets or sets the maximum duration, in seconds, to wait before retrying an operation.
        /// </summary>
        public int? MaximumBackOffInSeconds { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReliabilityValue"/> class, allowing configuration of retry and
        /// backoff parameters for reliability mechanisms.
        /// </summary>
        /// <param name="maximumNumberOfRetries">The maximum number of retry attempts. If <c>null</c>, the default retry count will be used.</param>
        /// <param name="minimumBackOffInSeconds">The minimum backoff duration, in seconds, between retry attempts. If <c>null</c>, the default minimum
        /// backoff will be used.</param>
        /// <param name="deltaBackOffInSeconds">The incremental backoff duration, in seconds, added to the minimum backoff for each retry attempt. If
        /// <c>null</c>, the default delta backoff will be used.</param>
        /// <param name="maximumBackOffInSeconds">The maximum backoff duration, in seconds, allowed between retry attempts. If <c>null</c>, the default
        /// maximum backoff will be used.</param>
        public ReliabilityValue(int? maximumNumberOfRetries = null, int? minimumBackOffInSeconds = null, int? deltaBackOffInSeconds = null, int? maximumBackOffInSeconds = null)
        {
            MaximumNumberOfRetries = maximumNumberOfRetries;
            MinimumBackOffInSeconds = minimumBackOffInSeconds;
            DeltaBackOffInSeconds = deltaBackOffInSeconds;
            MaximumBackOffInSeconds = maximumBackOffInSeconds;
        }
    }
}

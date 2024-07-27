using HypeLab.MailEngine.Data.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace HypeLab.MailEngine.Data.Models.Impl.Credentials.ValueTypes
{
    /// <summary>
    /// Represents the reliability settings to use on HTTP Requests for SendGrid client.
    /// </summary>
    /// <param name="maximumNumberOfRetries"></param>
    /// <param name="minimumBackOffInSeconds"></param>
    /// <param name="deltaBackOffInSeconds"></param>
    /// <param name="maximumBackOffInSeconds"></param>
    public struct ReliabilityValue(int? maximumNumberOfRetries = null, int? minimumBackOffInSeconds = null, int? deltaBackOffInSeconds = null, int? maximumBackOffInSeconds = null)
    {
        /// <summary>
        /// Validates the reliability settings.
        /// </summary>
        /// <exception cref="InvalidReliabilityPropertiesException"></exception>
        [MemberNotNull(nameof(MaximumBackOffInSeconds), nameof(MaximumNumberOfRetries), nameof(DeltaBackOffInSeconds), nameof(MinimumBackOffInSeconds))]
        public readonly void Validate()
        {
            if (!MaximumNumberOfRetries.HasValue || !MinimumBackOffInSeconds.HasValue || !DeltaBackOffInSeconds.HasValue || !MaximumBackOffInSeconds.HasValue)
                throw new InvalidReliabilityPropertiesException("All reliability properties must be provided.");
        }
        /// <summary>
        /// The reliability settings to use on HTTP Requests.
        /// </summary>
        public int? MaximumNumberOfRetries { get; set; } = maximumNumberOfRetries;

        /// <summary>
        /// The minimum amount of time in seconds to wait between HTTP retries.
        /// </summary>
        public int? MinimumBackOffInSeconds { get; set; } = minimumBackOffInSeconds;

        /// <summary>
        /// The delta back off.
        /// </summary>
        public int? DeltaBackOffInSeconds { get; set; } = deltaBackOffInSeconds;

        /// <summary>
        /// The maximum amount of time in seconds to wait between HTTP retries. Max value of 30 seconds.
        /// </summary>
        public int? MaximumBackOffInSeconds { get; set; } = maximumBackOffInSeconds;
    }
}

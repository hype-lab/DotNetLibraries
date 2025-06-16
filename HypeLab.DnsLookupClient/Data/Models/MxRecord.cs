namespace HypeLab.DnsLookupClient.Data.Models
{
    /// <summary>
    /// Represents a DNS Mail Exchange (MX) record, which specifies the mail server responsible for receiving emails for
    /// a domain.
    /// </summary>
    /// <remarks>An MX record contains the domain name of the mail server (<see cref="Exchange"/>) and a
    /// priority value (<see cref="Preference"/>). The priority determines the order in which mail servers are used,
    /// with lower values indicating higher priority.</remarks>
    public class MxRecord : DnsRecord
    {
        /// <summary>
        /// Gets the name of the stock exchange associated with the entity.
        /// </summary>
        public string Exchange { get; }
        /// <summary>
        /// Gets the preference value associated with the current operation or configuration.
        /// </summary>
        public ushort Preference { get; }

        /// <summary>
        /// Represents a mail exchange (MX) record used in DNS to specify the mail server responsible for receiving
        /// emails for a domain.
        /// </summary>
        /// <param name="exchange">The domain name of the mail server. This value cannot be null or empty.</param>
        /// <param name="preference">The preference value of the mail server. Lower values indicate higher priority.</param>
        public MxRecord(string exchange, ushort preference)
        {
            Exchange = exchange;
            Preference = preference;
        }

        /// <summary>
        /// Returns a string representation of the object, including the preference and exchange values.
        /// </summary>
        /// <returns>A string that concatenates the preference and exchange values, separated by a space.</returns>
        public override string ToString()
        {
            return $"{Preference} {Exchange}";
        }
    }
}

using System.Net.Sockets;

namespace HypeLab.DnsLookupClient.Data.Clients
{
#pragma warning disable S2094 // Classes should not be empty
    /// <summary>
    /// Represents a specialized UDP client for use with the HypeLab framework.
    /// </summary>
    /// <remarks>This class extends the <see cref="UdpClient"/> class, providing a foundation for
    /// HypeLab-specific UDP communication. Additional functionality or customization may be implemented in derived
    /// classes.</remarks>
    public class HypeLabUdpClient : UdpClient { }
#pragma warning restore S2094 // Classes should not be empty
}

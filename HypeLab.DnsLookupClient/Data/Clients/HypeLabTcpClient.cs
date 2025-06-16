using System.Net.Sockets;

namespace HypeLab.DnsLookupClient.Data.Clients
{
#pragma warning disable S2094 // Classes should not be empty
    /// <summary>
    /// Represents a specialized TCP client for connecting to and communicating with HypeLab servers.
    /// </summary>
    /// <remarks>This class extends <see cref="System.Net.Sockets.TcpClient"/> and inherits its functionality.
    /// Use this class to establish and manage TCP connections specifically tailored for HypeLab server
    /// interactions.</remarks>
    public class HypeLabTcpClient : TcpClient { }
#pragma warning restore S2094 // Classes should not be empty
}

using HypeLab.DnsLookupClient.Data.Enums;
using HypeLab.DnsLookupClient.Data.Models;
using System;
using System.Threading.Tasks;

namespace HypeLab.DnsLookupClient.Data.Interfaces
{
    public interface ILookupClient : IDisposable
    {
        Task<DnsQueryResult> QueryAsync(string domain, DnsQueryType queryType);
        DnsQueryResult Query(string domain, DnsQueryType queryType);
    }
}

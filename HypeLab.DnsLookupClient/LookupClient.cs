using HypeLab.DnsLookupClient.Data.Clients;
using HypeLab.DnsLookupClient.Data.Enums;
using HypeLab.DnsLookupClient.Data.Interfaces;
using HypeLab.DnsLookupClient.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HypeLab.DnsLookupClient
{
    public class LookupClient : ILookupClient
    {
        private readonly HypeLabUdpClient _udpClient;

        public LookupClient(HypeLabUdpClient? udpClient = null)
        {
            _udpClient = udpClient ?? new HypeLabUdpClient();
        }

        public async Task<DnsQueryResult> QueryAsync(string domain, DnsQueryType queryType)
        {
            if (queryType != DnsQueryType.MX)
                throw new NotSupportedException("Only MX queries are supported in this custom implementation.");

            List<MxRecord> mxRecords = new List<MxRecord>();
            DnsRequest dnsRequest = new DnsRequest(domain, queryType);
            DnsResponse dnsResponse = await dnsRequest.ResolveAsync(_udpClient).ConfigureAwait(false);

            foreach (DnsRecord record in dnsResponse.Answers)
            {
                if (record is MxRecord mxRecord)
                {
                    mxRecords.Add(mxRecord);
                }
            }

            return new DnsQueryResult(mxRecords);
        }

        public DnsQueryResult Query(string domain, DnsQueryType queryType)
        {
            if (queryType != DnsQueryType.MX)
                throw new NotSupportedException("Only MX queries are supported in this custom implementation.");

            List<MxRecord> mxRecords = new List<MxRecord>();
            DnsRequest dnsRequest = new DnsRequest(domain, queryType);
            DnsResponse dnsResponse = dnsRequest.Resolve(_udpClient);

            foreach (DnsRecord record in dnsResponse.Answers)
            {
                if (record is MxRecord mxRecord)
                {
                    mxRecords.Add(mxRecord);
                }
            }

            return new DnsQueryResult(mxRecords);
        }

        #region disposing
        private bool _disposed;
        ~LookupClient()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
                _udpClient?.Dispose();

            _disposed = true;
        }
        #endregion
    }
}

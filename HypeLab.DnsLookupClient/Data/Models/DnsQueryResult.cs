using System.Collections.Generic;

namespace HypeLab.DnsLookupClient.Data.Models
{
    public class DnsQueryResult
    {
        public List<MxRecord> Answers { get; }
        public DnsQueryResult(List<MxRecord> answers)
        {
            Answers = answers;
        }
    }
}

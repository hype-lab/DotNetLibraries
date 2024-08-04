namespace HypeLab.DnsLookupClient.Data.Models
{
    public class MxRecord : DnsRecord
    {
        public string Exchange { get; }

        public MxRecord(string exchange)
        {
            Exchange = exchange;
        }
    }
}

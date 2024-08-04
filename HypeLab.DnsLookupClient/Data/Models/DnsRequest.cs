using HypeLab.DnsLookupClient.Data.Enums;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HypeLab.DnsLookupClient.Helpers.Const;
using HypeLab.DnsLookupClient.Data.Clients;

namespace HypeLab.DnsLookupClient.Data.Models
{
    internal class DnsRequest
    {
        private readonly string _domain;
        private readonly DnsQueryType _queryType;

        public DnsRequest(string domain, DnsQueryType queryType)
        {
            _domain = domain;
            _queryType = queryType;
        }

        public async Task<DnsResponse> ResolveAsync(HypeLabUdpClient udpClient)
        {
            udpClient.Connect(DnsLookupDefaults.DnsServer, DnsLookupDefaults.DnsServerPort);

            byte[] requestBytes = CreateDnsQueryPacket(_domain, _queryType);
            await udpClient.SendAsync(requestBytes, requestBytes.Length).ConfigureAwait(false);

            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, DnsLookupDefaults.DnsServerPort);
            byte[] responseBytes = udpClient.Receive(ref endpoint);

            return ParseDnsResponse(responseBytes);
        }

        public DnsResponse Resolve(HypeLabUdpClient udpClient)
        {
            udpClient.Connect(DnsLookupDefaults.DnsServer, DnsLookupDefaults.DnsServerPort);

            byte[] requestBytes = CreateDnsQueryPacket(_domain, _queryType);
            udpClient.Send(requestBytes, requestBytes.Length);

            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, DnsLookupDefaults.DnsServerPort);
            byte[] responseBytes = udpClient.Receive(ref endpoint);

            return ParseDnsResponse(responseBytes);
        }

        private byte[] CreateDnsQueryPacket(string domain, DnsQueryType queryType)
        {
            List<byte> packet = new List<byte>();

            // Transaction ID
            packet.AddRange(BitConverter.GetBytes((ushort)new Random().Next(ushort.MaxValue)));

            // Flags
            packet.AddRange(BitConverter.GetBytes((ushort)0x0100)); // Standard query

            // Questions
            packet.AddRange(BitConverter.GetBytes((ushort)1));

            // Answer RRs, Authority RRs, Additional RRs
            packet.AddRange(BitConverter.GetBytes((ushort)0));
            packet.AddRange(BitConverter.GetBytes((ushort)0));
            packet.AddRange(BitConverter.GetBytes((ushort)0));

            // Query
            foreach (string part in domain.Split('.'))
            {
                packet.Add((byte)part.Length);
                packet.AddRange(Encoding.UTF8.GetBytes(part));
            }

            packet.Add(0); // End of domain name

            // Query Type
            packet.AddRange(BitConverter.GetBytes((ushort)queryType));

            // Query Class
            packet.AddRange(BitConverter.GetBytes((ushort)1)); // IN (Internet)

            return packet.ToArray();
        }

        private DnsResponse ParseDnsResponse(byte[] responseBytes)
        {
            List<DnsRecord> answers = new List<DnsRecord>();

            // Skip the header and question sections
            int offset = 12;
            while (responseBytes[offset] != 0)
            {
                offset++;
            }
            offset += 5;

            // Read the answer section
            ushort answerCount = BitConverter.ToUInt16(responseBytes, 6);
            for (int i = 0; i < answerCount; i++)
            {
                // Skip the name
                while (responseBytes[offset] != 0)
                {
                    offset++;
                }
                offset += 1;

                // Read the type and class
                int type = BitConverter.ToUInt16(responseBytes, offset);
                offset += 2;
                //var classCode = BitConverter.ToUInt16(responseBytes, offset)
                offset += 2;

                // Skip the TTL
                offset += 4;

                // Read the data length
                int dataLength = BitConverter.ToUInt16(responseBytes, offset);
                offset += 2;

                // Read the data
                if (type == (ushort)DnsQueryType.MX)
                {
                    //var preference = BitConverter.ToUInt16(responseBytes, offset)
                    offset += 2;

                    StringBuilder exchange = new StringBuilder();
                    while (responseBytes[offset] != 0)
                    {
                        int length = responseBytes[offset];
                        offset++;
                        exchange.Append(Encoding.UTF8.GetString(responseBytes, offset, length));
                        offset += length;
                        if (responseBytes[offset] != 0)
                        {
                            exchange.Append('.');
                        }
                    }
                    offset++;

                    answers.Add(new MxRecord(exchange.ToString()));
                }
                else
                {
                    offset += dataLength;
                }
            }

            return new DnsResponse(answers);
        }
    }
}

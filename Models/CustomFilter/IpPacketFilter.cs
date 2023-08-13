using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace packetSniffer
{
    class IpPacketFilter:IPacketFilter
    {
        private IPAddress _ipSrcAddress;
        private IPAddress _ipDstAddress;

        public IpPacketFilter(IPAddress ipSrcAddress, IPAddress ipDstAddress)
        {
            _ipSrcAddress = ipSrcAddress;
            _ipDstAddress = ipDstAddress;
        }

        public IEnumerable<Packet> Filter(IEnumerable<Packet> packets)
        {
            if (_ipSrcAddress != null)
                packets = packets.Where(x => _ipSrcAddress.Equals(x.SourceIp));
            if (_ipDstAddress != null)
                packets = packets.Where(x => _ipDstAddress.Equals(x.DestinationIp));
            return packets;
        }
    }
}

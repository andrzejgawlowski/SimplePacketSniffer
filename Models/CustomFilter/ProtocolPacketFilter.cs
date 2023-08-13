using System.Collections.Generic;
using System.Linq;

namespace packetSniffer
{
    class ProtocolPacketFilter : IPacketFilter
    {
        private IEnumerable<Protocol> _protocol;

        public ProtocolPacketFilter(IEnumerable<Protocol> protocol)
        {
            _protocol = protocol;
        }

        public IEnumerable<Packet>Filter(IEnumerable<Packet> packets)
        {
           var list = new List<Packet>() { };

           foreach(var proto in _protocol)
           {
                list.AddRange(packets.Where(x => x.PacketType == proto));
           }
            return list;
        }
    }
}

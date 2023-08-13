using System.Collections.Generic;
using System.Linq;

namespace packetSniffer
{
    class LengthPacketFilter : IPacketFilter
    {
        private uint _packetLengthFrom;
        private uint _packetLengthTo;

        public LengthPacketFilter(string packetLengthFrom, string packetLengthTo)
        {
            uint.TryParse(packetLengthTo, out _packetLengthTo);
            uint.TryParse(packetLengthFrom, out _packetLengthFrom);
        }

        public IEnumerable<Packet> Filter(IEnumerable<Packet> packets)
        {
            if (_packetLengthFrom > 0 && _packetLengthTo > 0)
                return packets.Where(x => x.TotalLenght <= _packetLengthTo && x.TotalLenght >= _packetLengthFrom);
            else if (_packetLengthFrom > 0)
                return packets.Where(x => x.TotalLenght >= _packetLengthFrom);
            else if (_packetLengthTo > 0)
                return packets.Where(x => x.TotalLenght <= _packetLengthTo);
            else
                return packets;
        }
    }
}

using System.Collections.Generic;
namespace packetSniffer
{
    public interface IPacketFilter
    {
        IEnumerable<Packet> Filter(IEnumerable<Packet> packets);
    }
}

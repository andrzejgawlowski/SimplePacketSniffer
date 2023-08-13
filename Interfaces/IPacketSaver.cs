using System.Collections.Generic;

namespace packetSniffer
{
    public interface IPacketSaver
    {
        void Save(IEnumerable<Packet> packets);
    }
}

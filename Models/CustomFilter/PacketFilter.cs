using System.Collections.Generic;

namespace packetSniffer
{
    public class PacketFilter
    {
        private IPacketFilter _packetFilter;

        public PacketFilter(IPacketFilter packetFilter)
        {
            _packetFilter = packetFilter;
        }

        public IEnumerable<Packet> Filter(IEnumerable<Packet> packets)
        {
           return _packetFilter.Filter(packets);
        }

        public void SetFilter(IPacketFilter packetFilter)
        {
            _packetFilter = packetFilter;
        }

        public static void Reset(IEnumerable<Packet> packets)
        {
            UIControl.UpdateItemSourceList(packets);
            UIControl.ResetFilterOptions();
        }
    }
}

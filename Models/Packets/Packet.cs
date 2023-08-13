using System.Net;

namespace packetSniffer
{
    public abstract class Packet : IPacket
    {
        public uint Id { get; set; }

        public byte[] rawByteConent;

        public Protocol PacketType { get; set; }

        public IPAddress SourceIp { get; set; }

        public IPAddress DestinationIp { get; set; }

        public int TotalLenght { get; set; }

        protected Packet(byte[] rawByteConent, Protocol packetType, IPAddress sourceIP, IPAddress destinationIP, int totalLenght)
        {
            this.rawByteConent = rawByteConent;
            this.PacketType = packetType;
            SourceIp = sourceIP;
            DestinationIp = destinationIP;
            this.TotalLenght = totalLenght;
        }

        protected Packet(byte[] rawByteConent, IPAddress sourceIP, IPAddress destinationIP, int totalLenght)
        {
            this.rawByteConent = rawByteConent;
            SourceIp = sourceIP;
            DestinationIp = destinationIP;
            this.TotalLenght = totalLenght;
        }

        public abstract void Send(int count, int delay, System.Threading.CancellationToken token);
    }
}

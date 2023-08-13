namespace packetSniffer
{
    public interface IPacket
    {
        void Send(int count, int delay, System.Threading.CancellationToken token);
    }
}

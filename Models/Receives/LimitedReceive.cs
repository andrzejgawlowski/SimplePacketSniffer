using System;
using System.Collections.Generic;

namespace packetSniffer
{
    class LimitedReceive : IReceive
    {
        private int? memory;

        private int? count;

        private int totalPacketSize = 0;

        private int totalPacketCount = 0;

        public LimitedReceive()
        {
            memory = null;
            count = null;
        }
        public LimitedReceive(int? memory, int? count)
        {
            this.memory = memory;
            this.count = count;
        }

        public void Receive(IAsyncResult result, System.Threading.CancellationToken token, byte[] buffer, List<int> listenPorts, System.Threading.CancellationTokenSource source)
        {
            if (!token.IsCancellationRequested)
            {
                var packetReader = new PacketReader(buffer);
                var packet = packetReader.GetProcessedPacket();

                totalPacketSize += packet.TotalLenght;

                if (memory == 0 || totalPacketSize < memory)
                {

                    if (count == 0 || ++totalPacketCount <= count)
                    {
                        packet.Id = ++UIControl.recvCounter;
                        UIControl.DynamicUpdate(packet);
                        UIControl.UpdateTotalSize(ByteCalculator.CalculateByteString(packet.TotalLenght));
                        System.Threading.Thread.Sleep(5);
                    }
                    else
                    { source.Cancel(); source.Dispose(); UIControl.SniffTurnOff(); }
                 }
                else
                { source.Cancel(); source.Dispose(); UIControl.SniffTurnOff(); }
            }
            return;
        }
    }
}

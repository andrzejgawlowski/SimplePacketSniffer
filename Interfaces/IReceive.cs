using System;
using System.Collections.Generic;

namespace packetSniffer
{
    public interface IReceive
    {
        void Receive(IAsyncResult result, System.Threading.CancellationToken token, byte[] buffer, List<int>listenPorts, System.Threading.CancellationTokenSource source);
    }
}

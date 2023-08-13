using System.Collections.Generic;
using System.IO; 

namespace packetSniffer
{
    public class JsonPacketSaver : IPacketSaver
    {
        public void Save(IEnumerable<Packet> packets)
        {
            var options = new Newtonsoft.Json.JsonSerializerSettings();
            options.Converters.Add(new IpAddressNewtonsoftJsonConverter());
            var stringContent = Newtonsoft.Json.JsonConvert.SerializeObject(packets, options);
            using (var writer = new StreamWriter(PacketSaver.FilePath))
            {
                writer.Write(stringContent);
            }
        }
    }
}

using System;

namespace packetSniffer
{
    class IpAddressNewtonsoftJsonConverter : Newtonsoft.Json.JsonConverter<System.Net.IPAddress>
    {
        public override System.Net.IPAddress ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, System.Net.IPAddress existingValue, bool hasExistingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            return existingValue.MapToIPv4();
        }

        public override void WriteJson(Newtonsoft.Json.JsonWriter writer, System.Net.IPAddress value, Newtonsoft.Json.JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}

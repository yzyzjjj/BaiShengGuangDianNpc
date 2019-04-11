using ModelBase.Base.EnumConfig;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ModelBase.Models.Device
{
    public class DeviceBaseInfo
    {
        public int DeviceId { get; set; }
        public int ServerId { get; set; }
        public int GroupId { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }
        public bool Monitoring { get; set; }
        public int Frequency { get; set; }
        public string Instruction { get; set; }
        public bool Storage { get; set; }
    }

    public class DeviceInfo : DeviceBaseInfo
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public SocketState State { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public DeviceState DeviceState { get; set; }
        [JsonIgnore]
        public int ScriptId { get; set; }
    }

}

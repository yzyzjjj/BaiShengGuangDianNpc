using ModelBase.Base.EnumConfig;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ModelBase.Models.Device
{
    public class DeviceBaseInfo
    {
        public int Id { get; set; }
        public int DeviceId { get; set; }
        public int ServerId { get; set; }
        public int GroupId { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }
        public bool Monitoring { get; set; }
        public int Frequency { get; set; }
        public string Instruction { get; set; }
        public bool Storage { get; set; }

        public void Update(DeviceBaseInfo device)
        {
            Monitoring = device.Monitoring;
            Frequency = device.Frequency;
            Instruction = device.Instruction;
            Storage = device.Storage;
        }
    }

    public class DeviceInfo : DeviceBaseInfo
    {
        public string Code { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public SocketState State { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public DeviceState DeviceState { get; set; }
        [JsonIgnore]
        public int ScriptId { get; set; }
        /// <summary>
        /// 当前加工流程卡号
        /// </summary>
        public string FlowCard { get; set; } = string.Empty;
        /// <summary>
        /// 加工时间
        /// </summary>
        public string ProcessTime { get; set; } = string.Empty;
        /// <summary>
        /// 剩余加工时间
        /// </summary>
        public string LeftTime { get; set; } = string.Empty;
    }

}

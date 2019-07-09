

namespace GateProxyLinkServer.Base.Control
{
    public abstract class MessagePacket
    {
        public abstract ControlEnum ControlEnum { get; }
        /// <summary>
        /// 包头
        /// </summary>
        protected string Header => "f3";
        /// <summary>
        /// 功能码
        /// </summary>
        public abstract int FunctionCode { get; }

        /// <summary>
        /// 子功能码
        /// </summary>
        public virtual int SubFunctionCode { get; } = 0;
        public  int ValNum { get; set; }
        public  int InNum { get; set; }
        public  int OutNum { get; set; }

        public abstract string Serialize();
        public abstract dynamic Deserialize(string response);
    }
}

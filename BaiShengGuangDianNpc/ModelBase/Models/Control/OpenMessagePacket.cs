
namespace ModelBase.Models.Control
{
    public class OpenMessagePacket : MessagePacket
    {
        public override ControlEnum ControlEnum => ControlEnum.Open;

        public override int FunctionCode => throw new System.NotImplementedException();

        public override dynamic Deserialize(string response)
        {
            throw new System.NotImplementedException();
        }

        public override string Serialize()
        {
            throw new System.NotImplementedException();
        }
    }
}

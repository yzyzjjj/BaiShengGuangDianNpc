﻿using System;

namespace ModelBase.Models.Control
{
    public class CloseMessagePacket : MessagePacket
    {
        public override ControlEnum ControlEnum => ControlEnum.Close;

        public override int FunctionCode => throw new NotImplementedException();

        public override dynamic Deserialize(string response)
        {
            throw new NotImplementedException();
        }

        public override string Serialize()
        {
            throw new NotImplementedException();
        }
    }
}

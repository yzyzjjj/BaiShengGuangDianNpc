using System;
using System.Collections.Generic;

namespace ModelBase.Models.Result
{
    public class MessageResult : Result
    {
        public List<Tuple<int, string>> messages = new List<Tuple<int, string>>();
    }
}

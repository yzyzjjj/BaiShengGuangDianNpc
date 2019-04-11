using ModelBase.Base.Utils;
using ModelBase.Models.Device;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;

namespace ModelBase.Models.Result
{
    public class DataResult : Result
    {
        public List<object> datas = new List<object>();
    }
}

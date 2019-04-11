using System.ComponentModel;
using ModelBase.Base.EnumConfig;
using ModelBase.Base.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ModelBase.Models.Device
{

    public class DeviceErr: Result.Result
    {
        public DeviceErr(int deviceId, Error err)
        {
            DeviceId = deviceId;
            errno = err;
        }
        public int DeviceId;
    }

}

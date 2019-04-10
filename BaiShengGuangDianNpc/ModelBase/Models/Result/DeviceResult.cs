using System;
using System.Collections.Generic;
using ModelBase.Models.Device;

namespace ModelBase.Models.Result
{
    public class DeviceResult : Result
    {
        public List<DeviceInfo> datas = new List<DeviceInfo>();
    }

    public class DeviceUpdateResult : Result
    {
        public List<object> deviceModels = new List<object>();
        public List<object> firmwareLibraries = new List<object>();
        public List<object> hardwareLibraries = new List<object>();
        public List<object> applicationLibraries = new List<object>();
        public List<object> scriptVersions = new List<object>();
        public List<object> sites = new List<object>();
    }
}

﻿using ModelBase.Models.Device;
using System.Collections.Generic;

namespace ModelBase.Models.Result
{
    public class DeviceResult : Result
    {
        public List<DeviceInfo> datas = new List<DeviceInfo>();
    }

    public class DeviceUpdateResult : Result
    {
        public List<object> deviceCategories = new List<object>();
        public List<object> deviceModels = new List<object>();
        public List<object> firmwareLibraries = new List<object>();
        public List<object> hardwareLibraries = new List<object>();
        public List<object> applicationLibraries = new List<object>();
        public List<object> scriptVersions = new List<object>();
        public List<object> sites = new List<object>();
        public List<object> maintainers = new List<object>();
        public List<object> classes = new List<object>();
    }
}

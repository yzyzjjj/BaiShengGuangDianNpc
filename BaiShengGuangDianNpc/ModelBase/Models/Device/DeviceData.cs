using System.Collections.Generic;

namespace ModelBase.Models.Device
{
    public class DeviceData
    {
        public DeviceData()
        {
            vals = new List<int>();
            ins = new List<int>();
            outs = new List<int>();
        }
        public List<int> vals;
        public List<int> ins;
        public List<int> outs;
    }
    public class DeviceTrueData
    {
        public DeviceTrueData()
        {
            vals = new List<decimal>();
            ins = new List<decimal>();
            outs = new List<decimal>();
        }
        public List<decimal> vals;
        public List<decimal> ins;
        public List<decimal> outs;
    }
}

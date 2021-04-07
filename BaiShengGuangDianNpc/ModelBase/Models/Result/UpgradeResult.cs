using System.Collections.Generic;
using ModelBase.Models.Device;

namespace ModelBase.Models.Result
{
    public class UpgradeResult : DataErrResult
    {
        /// <summary>
        /// 0  默认  1 升级流程脚本  2 升级固件  3 升级应用层
        /// </summary>
        public int Type { get; set; }
    }
}
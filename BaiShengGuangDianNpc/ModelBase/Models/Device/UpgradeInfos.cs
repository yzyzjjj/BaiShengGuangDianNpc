using System.Collections.Generic;

namespace ModelBase.Models.Device
{
    public class UpgradeInfos
    {
        /// <summary>
        /// 0  默认  1 升级流程脚本  2 升级固件  3 升级应用层
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 升级列表
        /// </summary>
        public List<UpgradeInfo> Infos { get; set; } = new List<UpgradeInfo>();
    }
}

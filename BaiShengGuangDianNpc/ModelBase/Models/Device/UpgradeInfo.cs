using ModelBase.Base.EnumConfig;
using ModelBase.Base.Utils;
using Newtonsoft.Json;
using ServiceStack;
using System.Linq;

namespace ModelBase.Models.Device
{
    /// <summary>
    /// 单设备升级流程脚本
    /// </summary>
    public class UpgradeInfo
    {
        /// <summary>
        ///  脚本位置 0 本地  1 网络
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        ///  脚本路径
        /// </summary>
        public string FileUrl { get; set; }
        /// <summary>
        /// 设备id
        /// </summary>
        public int DeviceId { get; set; }
        /// <summary>
        ///  升级文件 不带crc
        /// </summary>
        public string UpgradeFile { get; set; }
        /// <summary>
        ///  升级文件 带crc
        /// </summary>
        [JsonIgnore]
        public string UpgradeFileCrc
        {
            get
            {
                if (UpgradeFile.IsNullOrEmpty() || !UpgradeFile.Contains(","))
                {
                    return string.Empty;
                }
                var file = UpgradeFile.Split(",").ToList();
                if (!file.Any())
                {
                    return string.Empty;
                }

                file.AddRange(CrcHelper.GetCrc16(file));
                return file.Join();
            }
        }

        public Error ErrNo { get; set; }
    }
}
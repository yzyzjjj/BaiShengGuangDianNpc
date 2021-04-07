namespace ModelBase.Models.Control
{
    public enum ControlEnum
    {
        /// <summary>
        /// 
        /// </summary>
        Default,
        /// <summary>
        /// 开启
        /// </summary>
        Open,
        /// <summary>
        /// 关闭
        /// </summary>
        Close,
        /// <summary>
        /// 重启
        /// </summary>
        Restart,
        /// <summary>
        /// 锁定/解锁 状态
        /// </summary>
        LockInfo,
        /// <summary>
        /// 锁定
        /// </summary>
        Lock,
        /// <summary>
        /// 解锁
        /// </summary>
        Unlock,
        /// <summary>
        /// 查设备数据
        /// </summary>
        DeviceInfo,
        /// <summary>
        /// 设定变量
        /// </summary>
        SetVal,
        /// <summary>
        /// 升级状态查询
        /// </summary>
        UpgradeState,
        /// <summary>
        /// 进入升级状态
        /// </summary>
        StartUpgrade,
        /// <summary>
        /// 升级流程脚本
        /// </summary>
        UpgradeScript,
    }
}

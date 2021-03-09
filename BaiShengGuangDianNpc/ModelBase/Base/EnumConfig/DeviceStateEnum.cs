namespace ModelBase.Base.EnumConfig
{
    public enum DeviceState
    {
        /// <summary>
        /// 未初始化
        /// </summary>
        UnInit = 0,
        /// <summary>
        /// 待机
        /// </summary>
        Waiting,
        /// <summary>
        /// 加工中
        /// </summary>
        Processing,
        /// <summary>
        /// 升级流程中
        /// </summary>
        UpgradeScript,
        /// <summary>
        /// 升级固件中
        /// </summary>
        UpgradeFirmware,
        /// <summary>
        /// 重启中
        /// </summary>
        Restart,
    }
}

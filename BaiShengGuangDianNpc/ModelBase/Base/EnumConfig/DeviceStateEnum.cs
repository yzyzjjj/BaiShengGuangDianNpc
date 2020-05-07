namespace ModelBase.Base.EnumConfig
{
    public enum DeviceState
    {
        /// <summary>
        /// 未初始化
        /// </summary>
        UnInit = 0,
        /// <summary>
        /// 待加工
        /// </summary>
        Waiting,
        /// <summary>
        /// 加工中
        /// </summary>
        Processing,
        /// <summary>
        /// 升级状态1 引导层
        /// </summary>
        Upgrade1,
        /// <summary>
        /// 升级状态2 
        /// </summary>
        Upgrade2,
        /// <summary>
        /// 升级流程中
        /// </summary>
        UpgradeScript,
    }
}

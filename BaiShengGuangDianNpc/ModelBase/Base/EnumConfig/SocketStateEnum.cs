namespace ModelBase.Base.EnumConfig
{
    public enum SocketState
    {
        /// <summary>
        /// 未初始化
        /// </summary>
        UnInit = 0,
        /// <summary>
        /// 初始化失败
        /// </summary>
        Fail,
        /// <summary>
        /// 初始化失败
        /// </summary>
        Success,
        /// <summary>
        /// 已连接
        /// </summary>
        Connected,
        /// <summary>
        /// 已断开
        /// </summary>
        Close,
        /// <summary>
        /// 重连中
        /// </summary>
        Connecting,

    }
}

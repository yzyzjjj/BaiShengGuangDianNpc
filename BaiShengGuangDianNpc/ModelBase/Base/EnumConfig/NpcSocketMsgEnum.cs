using System;
using System.Collections.Generic;
using System.Text;

namespace ModelBase.Base.EnumConfig
{
    public enum NpcSocketMsgType
    {
        /// <summary>
        /// 
        /// </summary>
        Default = 0,
        /// <summary>
        /// 设备列表
        /// </summary>
        List,
        /// <summary>
        /// 添加设备
        /// </summary>
        Add,
        /// <summary>
        /// 删除设备
        /// </summary>
        Delete,
        /// <summary>
        /// 设置设备存储数据
        /// </summary>
        Storage,
        /// <summary>
        ///  设置设备监控频率
        /// </summary>
        Frequency,
        /// <summary>
        /// 发送消息  不返回结果
        /// </summary>
        Send,
        /// <summary>
        /// 发送消息  并返回结果
        /// </summary>
        SendBack,
        /// <summary>
        /// 返回
        /// </summary>
        Success,
        /// <summary>
        /// 心跳
        /// </summary>
        Heart,
        /// <summary>
        /// 心跳返回
        /// </summary>
        HeartSuccess,
    }
}

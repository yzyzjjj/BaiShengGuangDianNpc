﻿using System.ComponentModel;

namespace ModelBase.Base.ServerConfig.Enum
{
    public enum Error
    {
        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        Success,
        /// <summary>
        /// 失败
        /// </summary>
        [Description("失败")]
        Fail,
        /// <summary>
        /// 服务器异常
        /// </summary>
        [Description("服务器异常")]
        ExceptionHappen,
        /// <summary>
        /// 数据解析失败
        /// </summary>
        [Description("数据解析失败")]
        AnalysisFail,
        #region 设备相关 1000 - 1999
        /// <summary>
        /// 设备不存在
        /// </summary>
        [Description("设备不存在")]
        DeviceNotExist = 1000,
        /// <summary>
        /// 设备已存在
        /// </summary>
        [Description("设备已存在")]
        DeviceIsExist = 1001,
        /// <summary>
        /// 设备连接异常
        /// </summary>
        [Description("设备连接异常")]
        DeviceException = 1002,
        /// <summary>
        /// 指令错误
        /// </summary>
        [Description("指令错误")]
        InstructionError = 1003,
        /// <summary>
        /// NPC服务器不存在
        /// </summary>
        [Description("NPC服务器不存在")]
        NpcServerNotExist = 1004,
        /// <summary>
        /// NPC服务器已存在
        /// </summary>
        [Description("NPC服务器已存在")]
        NpcServerIsExist = 1005,
        /// <summary>
        /// 设备类型不存在
        /// </summary>
        [Description("设备类型不存在")]
        DeviceCategoryNotExist = 1006,
        /// <summary>
        /// 设备类型已存在
        /// </summary>
        [Description("设备类型已存在")]
        DeviceCategoryIsExist = 1007,
        /// <summary>
        /// 设备型号不存在
        /// </summary>
        [Description("设备型号不存在")]
        DeviceModelNotExist = 1006,
        /// <summary>
        /// 设备型号已存在
        /// </summary>
        [Description("设备型号已存在")]
        DeviceModelIsExist = 1007,
        /// <summary>
        /// 固件版本不存在
        /// </summary>
        [Description("固件版本不存在")]
        FirmwareLibraryNotExist = 1008,
        /// <summary>
        /// 固件版本已存在
        /// </summary>
        [Description("固件版本已存在")]
        FirmwareLibraryIsExist = 1009,
        /// <summary>
        /// 硬件版本不存在
        /// </summary>
        [Description("硬件版本不存在")]
        HardwareLibraryNotExist = 1010,
        /// <summary>
        /// 硬件版本已存在
        /// </summary>
        [Description("硬件版本已存在")]
        HardwareLibraryIsExist = 1011,
        /// <summary>
        /// 流程脚本版本不存在
        /// </summary>
        [Description("流程脚本版本不存在")]
        ProcessLibraryNotExist = 1012,
        /// <summary>
        /// 流程脚本版本已存在
        /// </summary>
        [Description("流程脚本版本已存在")]
        ProcessLibraryIsExist = 1013,
        /// <summary>
        /// 场地不存在
        /// </summary>
        [Description("场地不存在")]
        SiteNotExist = 1014,
        /// <summary>
        /// 场地已存在
        /// </summary>
        [Description("场地已存在")]
        SiteIsExist = 1015,
        /// <summary>
        /// 该IP和端口已被占用
        /// </summary>
        [Description("该IP和端口已被占用")]
        IpPortIsExist = 1016,
        /// <summary>
        /// 非法IP
        /// </summary>
        [Description("非法IP")]
        IpInvalid = 1017,
        /// <summary>
        /// 非法端口
        /// </summary>
        [Description("非法端口")]
        PortInvalid = 1018,

        /// <summary>
        /// 流程卡不存在
        /// </summary>
        [Description("流程卡不存在")]
        FlowcardLibraryNotExist = 1019,
        /// <summary>
        /// 流程卡已存在
        /// </summary>
        [Description("流程卡已存在")]
        FlowcardLibraryIsExist = 1020,
        /// <summary>
        /// 工序不存在
        /// </summary>
        [Description("工序不存在")]
        ProcessStepNotExist = 1021,
        /// <summary>
        /// 工序已存在
        /// </summary>
        [Description("工序已存在")]
        ProcessStepIsExist = 1022,
        /// <summary>
        /// 计划号(产品型号)不存在
        /// </summary>
        [Description("计划号(产品型号)不存在")]
        ProductionProcessLibraryNotExist = 1023,
        /// <summary>
        /// 计划号(产品型号)已存在
        /// </summary>
        [Description("计划号(产品型号)已存在")]
        ProductionProcessLibraryIsExist = 1024,
        /// <summary>
        /// 原料批号不存在
        /// </summary>
        [Description("原料批号不存在")]
        RawMateriaNotExist = 1025,
        /// <summary>
        /// 原料批号已存在
        /// </summary>
        [Description("原料批号已存在")]
        RawMateriaIsExist = 1026,
        /// <summary>
        /// 原料规格不存在
        /// </summary>
        [Description("原料规格不存在")]
        RawMateriaSpecificationNotExist = 1027,
        /// <summary>
        /// 原料规格已存在
        /// </summary>
        [Description("原料规格已存在")]
        RawMateriaSpecificationIsExist = 1028,
        /// <summary>
        /// 加工人不存在
        /// </summary>
        [Description("加工人不存在")]
        ProcessorNotExist = 1029,
        /// <summary>
        /// 加工人已存在
        /// </summary>
        [Description("加工人已存在")]
        ProcessorIsExist = 1030,
        /// <summary>
        /// 检验员不存在
        /// </summary>
        [Description("检验员不存在")]
        SurveyorNotExist = 1031,
        /// <summary>
        /// 检验员已存在
        /// </summary>
        [Description("检验员已存在")]
        SurveyorIsExist = 1032,
        /// <summary>
        /// 输入输出变量不存在
        /// </summary>
        [Description("输入输出变量不存在")]
        DataNameDictionaryNotExist = 1033,
        /// <summary>
        /// 输入输出变量已存在
        /// </summary>
        [Description("输入输出变量已存在")]
        DataNameDictionaryIsExist = 1034,
        /// <summary>
        /// 工艺数据不存在
        /// </summary>
        [Description("工艺数据不存在")]
        ProcessDataNotExist = 1035,
        /// <summary>
        /// 工艺数据已存在
        /// </summary>
        [Description("工艺数据已存在")]
        ProcessDataIsExist = 1036,
        /// <summary>
        /// 工艺编号不存在
        /// </summary>
        [Description("工艺编号不存在")]
        ProcessManagementNotExist = 1037,
        /// <summary>
        /// 工艺编号已存在
        /// </summary>
        [Description("工艺编号已存在")]
        ProcessManagementIsExist = 1038,
        /// <summary>
        /// 故障类型不存在
        /// </summary>
        [Description("故障类型不存在")]
        FaultTypeNotExist = 1039,
        /// <summary>
        /// 故障类型已存在
        /// </summary>
        [Description("故障类型已存在")]
        FaultTypeIsExist = 1040,
        /// <summary>
        /// 故障设备不存在
        /// </summary>
        [Description("故障设备不存在")]
        FaultDeviceNotExist = 1041,
        /// <summary>
        /// 故障设备已存在
        /// </summary>
        [Description("故障设备已存在")]
        FaultDeviceIsExist = 1042,
        /// <summary>
        /// 常见故障解决方案不存在
        /// </summary>
        [Description("常见故障解决方案不存在")]
        UsuallyFaultNotExist = 1043,
        /// <summary>
        /// 常见故障解决方案已存在
        /// </summary>
        [Description("常见故障解决方案已存在")]
        UsuallyFaultIsExist = 1044,
        /// <summary>
        /// 维修记录不存在
        /// </summary>
        [Description("维修记录不存在")]
        RepairRecordNotExist = 1045,
        /// <summary>
        /// 维修记录已存在
        /// </summary>
        [Description("维修记录已存在")]
        RepairRecordIsExist = 1046,
        #endregion

        #region 管理后台  3000 - 3999
        /// <summary>
        /// 账号不存在
        /// </summary>
        [Description("账号不存在")]
        AccountNotExist = 3000,
        /// <summary>
        /// 账号已存在
        /// </summary>
        [Description("账号已存在")]
        AccountIsExist = 3001,
        /// <summary>
        /// 密码错误
        /// </summary>
        [Description("密码错误")]
        PasswordError = 3002,

        #endregion
    }
}
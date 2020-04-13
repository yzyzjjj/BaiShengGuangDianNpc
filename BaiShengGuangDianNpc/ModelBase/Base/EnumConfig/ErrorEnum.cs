﻿using System.ComponentModel;

namespace ModelBase.Base.EnumConfig
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
        /// <summary>
        /// 参数错误
        /// </summary>
        [Description("参数错误")]
        ParamError,
        /// <summary>
        /// 服务繁忙
        /// </summary>
        [Description("服务繁忙")]
        ServerBusy,
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
        /// 应用层版本不存在
        /// </summary>
        [Description("应用层版本不存在")]
        ApplicationLibraryNotExist = 1012,
        /// <summary>
        /// 应用层版本已存在
        /// </summary>
        [Description("应用层版本已存在")]
        ApplicationLibraryIsExist = 1013,
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
        FlowCardLibraryNotExist = 1019,
        /// <summary>
        /// 流程卡已存在
        /// </summary>
        [Description("流程卡已存在")]
        FlowCardLibraryIsExist = 1020,
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
        ProductionLibraryNotExist = 1023,
        /// <summary>
        /// 计划号(产品型号)已存在
        /// </summary>
        [Description("计划号(产品型号)已存在")]
        ProductionLibraryIsExist = 1024,
        /// <summary>
        /// 原料批号不存在
        /// </summary>
        [Description("原料批号不存在")]
        RawMaterialNotExist = 1025,
        /// <summary>
        /// 原料批号已存在
        /// </summary>
        [Description("原料批号已存在")]
        RawMaterialIsExist = 1026,
        /// <summary>
        /// 原料规格不存在
        /// </summary>
        [Description("原料规格不存在")]
        RawMaterialSpecificationNotExist = 1027,
        /// <summary>
        /// 原料规格已存在
        /// </summary>
        [Description("原料规格已存在")]
        RawMaterialSpecificationIsExist = 1028,
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
        /// 变量不存在
        /// </summary>
        [Description("变量不存在")]
        DataNameDictionaryNotExist = 1033,
        /// <summary>
        /// 变量已存在
        /// </summary>
        [Description("变量已存在")]
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
        /// 与故障指派维修工不符
        /// </summary>
        [Description("与故障指派人不符")]
        FaultDeviceRepairMaintainerError = 1042,
        /// <summary>
        /// 常见故障不存在
        /// </summary>
        [Description("常见故障不存在")]
        UsuallyFaultNotExist = 1043,
        /// <summary>
        /// 常见故障已存在
        /// </summary>
        [Description("常见故障已存在")]
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
        /// <summary>
        /// 数据类型不存在
        /// </summary>
        [Description("数据类型不存在")]
        VariableTypeNotExist = 1047,
        /// <summary>
        /// 数据类型已存在
        /// </summary>
        [Description("数据类型已存在")]
        VariableTypeIsExist = 1048,
        /// <summary>
        /// 脚本版本不存在
        /// </summary>
        [Description("流程脚本版本不存在")]
        ScriptVersionNotExist = 1049,
        /// <summary>
        /// 脚本版本已存在
        /// </summary>
        [Description("流程脚本版本已存在")]
        ScriptVersionIsExist = 1050,
        /// <summary>
        /// 常用变量类型不存在
        /// </summary>
        [Description("常用变量类型不存在")]
        UsuallyDictionaryTypeNotExist = 1051,
        /// <summary>
        /// 常用变量类型已存在
        /// </summary>
        [Description("常用变量类型已存在")]
        UsuallyDictionaryTypeIsExist = 1052,
        /// <summary>
        /// 常用变量类型不存在
        /// </summary>
        [Description("常用变量不存在")]
        UsuallyDictionaryNotExist = 1053,
        /// <summary>
        /// 常用变量类型已存在
        /// </summary>
        [Description("常用变量已存在")]
        UsuallyDictionaryIsExist = 1054,
        /// <summary>
        /// 数据地址重复
        /// </summary>
        [Description("数据地址重复")]
        PointerAddressIsExist = 1055,
        /// <summary>
        /// 同一产品型号和同一机台号不能添加到不同的工艺编号中
        /// </summary>
        [Description("同一产品型号和同一机台号不能添加到不同的工艺编号中")]
        ProcessManagementAddError = 1056,
        /// <summary>
        /// 设备工序已存在
        /// </summary>
        [Description("设备工序已存在")]
        DeviceProcessStepIsExist = 1057,
        /// <summary>
        /// 设备工序不存在
        /// </summary>
        [Description("设备工序不存在")]
        DeviceProcessStepNotExist = 1058,
        /// <summary>
        /// 车间已存在
        /// </summary>
        [Description("车间已存在")]
        WorkshopIsExist = 1059,
        /// <summary>
        /// 车间不存在
        /// </summary>
        [Description("车间不存在")]
        WorkshopNotExist = 1060,
        /// <summary>
        /// 设备状态异常不能设置工艺数据
        /// </summary>
        [Description("设备状态异常不能设置工艺数据")]
        DeviceStateErrorNotSet = 1061,
        /// <summary>
        /// 加工中设备不能设置工艺数据
        /// </summary>
        [Description("加工中设备不能设置工艺数据")]
        ProcessingNotSet = 1062,
        /// <summary>
        /// 未开始加工
        /// </summary>
        [Description("该工序未开始加工")]
        ProcessNotStart = 1063,
        /// <summary>
        /// 流程卡类型已存在
        /// </summary>
        [Description("流程卡类型已存在")]
        FlowCardTypeIsExist = 1064,
        /// <summary>
        /// 流程卡类型不存在
        /// </summary>
        [Description("流程卡类型不存在")]
        FlowCardTypeNotExist = 1065,
        /// <summary>
        /// 机台号存在,请选择后上报
        /// </summary>
        [Description("机台号存在,请选择后上报")]
        ReportDeviceCodeIsExist = 1066,
        /// <summary>
        /// 设备型号不存在
        /// </summary>
        [Description("设备型号不存在")]
        DeviceModelNotExist = 1067,
        /// <summary>
        /// 设备型号已存在
        /// </summary>
        [Description("设备型号已存在")]
        DeviceModelIsExist = 1068,
        /// <summary>
        /// 设备型号使用该类型中,无法删除
        /// </summary>
        [Description("设备型号使用该类型中,无法删除")]
        DeviceModelUseDeviceCategory = 1069,
        /// <summary>
        /// 设备型号使用该场地中,无法删除
        /// </summary>
        [Description("设备使用该场地中,无法删除")]
        DeviceLibraryUseSite = 1070,
        /// <summary>
        /// 设备使用该固件中,无法删除
        /// </summary>
        [Description("设备使用该固件中,无法删除")]
        DeviceLibraryUseFirmware = 1071,
        /// <summary>
        /// 设备使用该固件中,无法删除
        /// </summary>
        [Description("设备使用该硬件中,无法删除")]
        DeviceLibraryUseHardware = 1072,
        /// <summary>
        /// 设备使用该应用层中,无法删除
        /// </summary>
        [Description("设备使用该应用层中,无法删除")]
        DeviceLibraryUseApplication = 1073,
        /// <summary>
        /// 故障设备使用该类型中,无法删除
        /// </summary>
        [Description("故障设备使用该类型中,无法删除")]
        FaultDeviceUseFaultType = 1074,
        /// <summary>
        /// 设备工序使用该类型中,无法删除
        /// </summary>
        [Description("设备工序使用该类型中,无法删除")]
        DeviceProcessStepUseDeviceCategory = 1075,
        /// <summary>
        /// 点检计划不存在
        /// </summary>
        [Description("点检计划不存在")]
        SpotCheckPlanNotExist = 1076,
        /// <summary>
        /// 点检计划已存在
        /// </summary>
        [Description("点检计划已存在")]
        SpotCheckPlanIsExist = 1077,
        /// <summary>
        /// 点检计划已存在
        /// </summary>
        [Description("请输入点检计划名")]
        SpotCheckPlanNotEmpty = 1078,
        /// <summary>
        /// 点检项不存在
        /// </summary>
        [Description("点检项不存在")]
        SpotCheckItemNotExist = 1079,
        /// <summary>
        /// 点检项已存在
        /// </summary>
        [Description("点检项已存在")]
        SpotCheckItemIsExist = 1080,
        /// <summary>
        /// 点检项重复
        /// </summary>
        [Description("点检项重复")]
        SpotCheckItemDuplicate = 1081,
        /// <summary>
        /// 请输入点检项名
        /// </summary>
        [Description("请输入点检项名")]
        SpotCheckItemNotEmpty = 1082,
        /// <summary>
        /// 点检设备已设置该计划
        /// </summary>
        [Description("点检设备已设置该计划")]
        SpotCheckDeviceHaveThisPlan = 1083,
        /// <summary>
        /// 点检设备没有设置该计划
        /// </summary>
        [Description("点检设备没有设置该计划")]
        SpotCheckDeviceNotHaveThisPlan = 1084,
        /// <summary>
        /// 点检项已过期
        /// </summary>
        [Description("点检项已过期")]
        SpotCheckDeviceExpired = 1085,
        /// <summary>
        /// 点检记录已存在
        /// </summary>
        [Description("点检记录已存在")]
        SpotCheckLogIsExist = 1086,
        /// <summary>
        /// 点检记录不存在
        /// </summary>
        [Description("点检记录不存在")]
        SpotCheckLogNotExist = 1087,

        /// <summary>
        /// 货品位置已存在
        /// </summary>
        [Description("货品位置已存在")]
        MaterialSiteIsExist = 1088,
        /// <summary>
        /// 货品位置不存在
        /// </summary>
        [Description("货品位置不存在")]
        MaterialSiteNotExist = 1089,
        /// <summary>
        /// 请输入货品位置
        /// </summary>
        [Description("请输入货品位置")]
        MaterialSiteNotEmpty = 1090,
        /// <summary>
        /// 货品位置重复
        /// </summary>
        [Description("货品位置重复")]
        MaterialSiteDuplicate = 1091,

        /// <summary>
        /// 货品类别已存在
        /// </summary>
        [Description("货品类别已存在")]
        MaterialCategoryIsExist = 1092,
        /// <summary>
        /// 货品类别不存在
        /// </summary>
        [Description("货品类别不存在")]
        MaterialCategoryNotExist = 1093,
        /// <summary>
        /// 请输入货品类别
        /// </summary>
        [Description("请输入货品类别")]
        MaterialCategoryNotEmpty = 1094,
        /// <summary>
        /// 货品类别重复
        /// </summary>
        [Description("货品类别重复")]
        MaterialCategoryDuplicate = 1095,

        /// <summary>
        /// 货品名称已存在
        /// </summary>
        [Description("货品名称已存在")]
        MaterialNameIsExist = 1096,
        /// <summary>
        /// 货品名称不存在
        /// </summary>
        [Description("货品名称不存在")]
        MaterialNameNotExist = 1097,
        /// <summary>
        /// 请输入货品名称
        /// </summary>
        [Description("请输入货品名称")]
        MaterialNameNotEmpty = 1098,
        /// <summary>
        /// 货品名称重复
        /// </summary>
        [Description("货品名称重复")]
        MaterialNameDuplicate = 1099,

        /// <summary>
        /// 货品供应商已存在
        /// </summary>
        [Description("货品供应商已存在")]
        MaterialSupplierIsExist = 1100,
        /// <summary>
        /// 货品供应商不存在
        /// </summary>
        [Description("货品供应商不存在")]
        MaterialSupplierNotExist = 1101,
        /// <summary>
        /// 请输入货品供应商
        /// </summary>
        [Description("请输入货品供应商")]
        MaterialSupplierNotEmpty = 1102,
        /// <summary>
        /// 货品供应商重复
        /// </summary>
        [Description("货品供应商重复")]
        MaterialSupplierDuplicate = 1103,

        /// <summary>
        /// 货品规格已存在
        /// </summary>
        [Description("货品规格已存在")]
        MaterialSpecificationIsExist = 1104,
        /// <summary>
        /// 货品规格不存在
        /// </summary>
        [Description("货品规格不存在")]
        MaterialSpecificationNotExist = 1105,
        /// <summary>
        /// 请输入货品规格
        /// </summary>
        [Description("请输入货品规格")]
        MaterialSpecificationNotEmpty = 1106,
        /// <summary>
        /// 货品规格重复
        /// </summary>
        [Description("货品规格重复")]
        MaterialSpecificationDuplicate = 1107,

        /// <summary>
        /// 货品编号已存在
        /// </summary>
        [Description("货品编号已存在")]
        MaterialBillIsExist = 1108,
        /// <summary>
        /// 货品编号不存在
        /// </summary>
        [Description("货品编号不存在")]
        MaterialBillNotExist = 1109,
        /// <summary>
        /// 请输入货品编号
        /// </summary>
        [Description("请输入货品编号")]
        MaterialBillNotEmpty = 1110,
        /// <summary>
        /// 货品编号重复
        /// </summary>
        [Description("货品编号重复")]
        MaterialBillDuplicate = 1111,
        /// <summary>
        /// 相同规格、位置的货品编号已存在
        /// </summary>
        [Description("相同规格、位置的货品编号已存在")]
        MaterialBillSpecificationSiteIsExist = 1112,
        /// <summary>
        /// 相同规格、位置的货品编号重复
        /// </summary>
        [Description("相同规格、位置的货品编号重复")]
        MaterialBillSpecificationSiteDuplicate = 1113,

        /// <summary>
        /// 计划已存在
        /// </summary>
        [Description("计划已存在")]
        ProductionPlanIsExist = 1114,
        /// <summary>
        /// 计划不存在
        /// </summary>
        [Description("计划不存在")]
        ProductionPlanNotExist = 1115,
        /// <summary>
        /// 请输入计划
        /// </summary>
        [Description("请输入计划")]
        ProductionPlanNotEmpty = 1116,
        ///// <summary>
        ///// 计划重复
        ///// </summary>
        //[Description("计划重复")]
        //ProductionPlanDuplicate = 1117,

        /// <summary>
        /// 请输入货品清单
        /// </summary>
        [Description("请输入货品清单")]
        MaterialManagementNotEmpty = 1117,
        /// <summary>
        /// 库存不足
        /// </summary>
        [Description("库存不足")]
        MaterialManagementLess = 1118,

        /// <summary>
        /// 无法删除已领用货品
        /// </summary>
        [Description("无法删除已领用货品")]
        ProductionPlanBillConsumed = 1119,
        /// <summary>
        /// 计划不包含该货品
        /// </summary>
        [Description("计划不包含该货品")]
        ProductionPlanBillNotExist = 1120,
        /// <summary>
        /// 退回数量超过实际用量
        /// </summary>
        [Description("退回数量超过实际用量")]
        ProductionPlanBillActualConsumeLess = 1121,

        /// <summary>
        /// 6s分组已存在
        /// </summary>
        [Description("6s分组已存在")]
        _6sGroupIsExist = 1122,

        /// <summary>
        /// 6s分组不存在
        /// </summary>
        [Description("6s分组不存在")]
        _6sGroupNotExist = 1123,

        /// <summary>
        /// 6s分组名称不能为空
        /// </summary>
        [Description("6s分组名称不能为空")]
        _6sGroupNotEmpty = 1124,

        /// <summary>
        /// 6s检查项已存在
        /// </summary>
        [Description("6s检查项已存在")]
        _6sItemIsExist = 1125,

        /// <summary>
        /// 6s检查项不存在
        /// </summary>
        [Description("6s检查项不存在")]
        _6sItemNotExist = 1126,

        /// <summary>
        /// 6s检查项不能为空
        /// </summary>
        [Description("6s检查项不能为空")]
        _6sItemNotEmpty = 1127,

        /// <summary>
        /// 6s检查日志已存在
        /// </summary>
        [Description("6s检查日志已存在")]
        _6sLogIsExist = 1128,

        /// <summary>
        /// 6s检查日志不存在
        /// </summary>
        [Description("6s检查日志不存在")]
        _6sLogNotExist = 1129,

        /// <summary>
        /// 生产任务模块已存在
        /// </summary>
        [Description("任务模块已存在")]
        ManufactureTaskModuleIsExist = 1130,

        /// <summary>
        /// 生产任务模块不存在
        /// </summary>
        [Description("任务模块不存在")]
        ManufactureTaskModuleNotExist = 1131,

        /// <summary>
        /// 检验配置单已存在
        /// </summary>
        [Description("检验配置单已存在")]
        ManufactureCheckIsExist = 1132,

        /// <summary>
        /// 检验配置单不存在
        /// </summary>
        [Description("检验配置单不存在")]
        ManufactureCheckNotExist = 1133,

        /// <summary>
        /// 检验配置单名称不能为空
        /// </summary>
        [Description("检验配置单名称不能为空")]
        ManufactureCheckNotEmpty = 1134,

        /// <summary>
        /// 检验项已存在
        /// </summary>
        [Description("检验项已存在")]
        ManufactureCheckItemIsExist = 1135,

        /// <summary>
        /// 检验项不存在
        /// </summary>
        [Description("检验项不存在")]
        ManufactureCheckItemNotExist = 1136,

        /// <summary>
        /// 检验流程不能为空
        /// </summary>
        [Description("检验流程不能为空")]
        ManufactureCheckItemNotEmpty = 1137,

        /// <summary>
        /// 任务配置单已存在
        /// </summary>
        [Description("任务配置单已存在")]
        ManufactureTaskIsExist = 1138,

        /// <summary>
        /// 任务配置单不存在
        /// </summary>
        [Description("任务配置单不存在")]
        ManufactureTaskNotExist = 1139,

        /// <summary>
        /// 任务配置单名称不能为空
        /// </summary>
        [Description("任务配置单名称不能为空")]
        ManufactureTaskNotEmpty = 1140,

        /// <summary>
        /// 任务已存在
        /// </summary>
        [Description("任务已存在")]
        ManufactureTaskItemIsExist = 1141,

        /// <summary>
        /// 任务不存在
        /// </summary>
        [Description("任务不存在")]
        ManufactureTaskItemNotExist = 1142,

        /// <summary>
        /// 任务名不能为空
        /// </summary>
        [Description("任务名不能为空")]
        ManufactureTaskItemNotEmpty = 1143,

        /// <summary>
        /// 任务顺序重复
        /// </summary>
        [Description("任务顺序重复")]
        ManufactureTaskItemOrderDuplicate = 1144,

        /// <summary>
        /// 任务关联错误
        /// </summary>
        [Description("任务关联错误")]
        ManufactureTaskItemRelationError = 1145,

        /// <summary>
        /// 检验单需先关联任务
        /// </summary>
        [Description("检验单需先关联任务")]
        ManufactureCheckItemNoRelation = 1146,

        /// <summary>
        /// 计划配置单已存在
        /// </summary>
        [Description("计划配置单已存在")]
        ManufacturePlanIsExist = 1147,

        /// <summary>
        /// 计划配置单不存在
        /// </summary>
        [Description("计划配置单不存在")]
        ManufacturePlanNotExist = 1148,

        /// <summary>
        /// 计划已下发
        /// </summary>
        [Description("计划已下发")]
        ManufacturePlaneAssignState = 1149,

        /// <summary>
        /// 非待下发计划无法修改
        /// </summary>
        [Description("非待下发计划无法修改")]
        ManufacturePlaneChangeState = 1150,

        /// <summary>
        /// 计划名不能为空
        /// </summary>
        [Description("计划名不能为空")]
        ManufacturePlanNotEmpty = 1151,

        /// <summary>
        /// 请先配置任务
        /// </summary>
        [Description("请先配置任务")]
        ManufacturePlaneNoTask = 1152,

        /// <summary>
        /// 生产日志不存在
        /// </summary>
        [Description("生产日志不存在")]
        ManufactureLogNotExist = 1153,

        /// <summary>
        /// 请选择计划
        /// </summary>
        [Description("请选择计划")]
        ManufactureLogSelectPlan = 1154,

        /// <summary>
        /// 分组已存在
        /// </summary>
        [Description("分组已存在")]
        ManufactureGroupIsExist = 1155,

        /// <summary>
        /// 分组不存在
        /// </summary>
        [Description("分组不存在")]
        ManufactureGroupNotExist = 1156,

        /// <summary>
        /// 分组名不能为空
        /// </summary>
        [Description("分组名不能为空")]
        ManufactureGroupNotEmpty = 1157,

        /// <summary>
        /// 员工已存在
        /// </summary>
        [Description("员工已存在")]
        ManufactureProcessorIsExist = 1158,

        /// <summary>
        /// 员工不存在
        /// </summary>
        [Description("员工不存在")]
        ManufactureProcessorNotExist = 1159,

        /// <summary>
        /// 分组名不能为空
        /// </summary>
        [Description("分组名不能为空")]
        ManufactureProcessorNotEmpty = 1160,

        /// <summary>
        /// 无任务
        /// </summary>
        [Description("无任务")]
        ManufactureNoTask = 1161,

        /// <summary>
        /// 无检验任务
        /// </summary>
        [Description("无检验任务")]
        ManufactureNoCheck = 1162,

        /// <summary>
        /// 任务状态错误
        /// </summary>
        [Description("任务状态错误")]
        ManufactureTaskStateError = 1163,

        /// <summary>
        /// 计划未下发
        /// </summary>
        [Description("计划未下发")]
        ManufacturePlaneNotAssign = 1164,

        /// <summary>
        /// 无法删除关联或被关联任务
        /// </summary>
        [Description("无法删除关联或被关联任务，请先修改")]
        ManufacturePlaneTaskDeleteHaveRelation = 1165,

        /// <summary>
        /// 无法删除关联或被关联任务
        /// </summary>
        [Description("无法停止关联或被关联任务，请先修改")]
        ManufacturePlaneTaskStopHaveRelation = 1166,

        /// <summary>
        /// 维修工已存在
        /// </summary>
        [Description("维修工已存在")]
        MaintainerIsExist = 1167,

        /// <summary>
        /// 维修工不存在
        /// </summary>
        [Description("维修工不存在")]
        MaintainerNotExist = 1168,
        /// <summary>
        /// 维修工重复
        /// </summary>
        [Description("维修工重复")]
        MaintainerDuplicate = 1169,

        /// <summary>
        /// 任务无法上移至进行中任务前
        /// </summary>
        [Description("任务无法上移至进行中任务前")]
        ManufacturePlaneTaskAfterDoing = 1170,
        /// <summary>
        /// 任务无法上移至检验中任务前
        /// </summary>
        [Description("任务无法上移至检验中任务前")]
        ManufacturePlaneTaskCheckAfterChecking = 1171,
        /// <summary>
        /// 任务无法上移至关联任务前
        /// </summary>
        [Description("任务无法上移至关联任务前")]
        ManufacturePlaneTaskAfterRelation = 1172,
        /// <summary>
        /// 无法上移非等待中/待检验任务
        /// </summary>
        [Description("无法上移非等待中/非待检验任务")]
        ManufacturePlaneTaskNotWait = 1173,
        /// <summary>
        /// 维修未完成
        /// </summary>
        [Description("维修未完成")]
        RepairRecordNotComplete = 1174,
        /// <summary>
        /// 关联任务非完成状态无法修改
        /// </summary>
        [Description("关联任务非完成状态无法修改")]
        ManufacturePlaneTaskNotDone = 1175,
        /// <summary>
        /// 关联任务非待返工状态无法修改
        /// </summary>
        [Description("关联任务非待返工/非待检验状态无法修改")]
        ManufacturePlaneTaskNotWaitRedo = 1176,
        /// <summary>
        /// 维修状态错误
        /// </summary>
        [Description("维修状态错误")]
        FaultDeviceStateError = 1177,






        ///// <summary>
        ///// 请输入货品清单
        ///// </summary>
        //[Description("请输入货品清单")]
        //ProductionPlanBillNotEmpty = 1118,
        ///// <summary>
        ///// 货品清单重复
        ///// </summary>
        //[Description("货品清单重复")]
        //ProductionPlanBillDuplicate = 1119,

        ///// <summary>
        ///// 流程卡使用该工序中,无法删除
        ///// </summary>
        //[Description("流程卡工序使用该工序中,无法删除")]
        //FlowCardLibraryUseDeviceProcessStep = 1076,
        ///// <summary>
        ///// 设备使用该应用层中,无法删除
        ///// </summary>
        //[Description("设备使用该应用层中,无法删除")]
        //DeviceLibraryUseApplication = 1075,
        ///// <summary>
        ///// 设备使用该应用层中,无法删除
        ///// </summary>
        //[Description("设备使用该应用层中,无法删除")]
        //DeviceLibraryUseApplication = 1076,
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
        /// <summary>
        /// 页面不存在
        /// </summary>
        [Description("页面不存在")]
        PageNotExist = 3003,
        /// <summary>
        /// Api不存在
        /// </summary>
        [Description("Api不存在")]
        ApiNotExist = 3004,
        /// <summary>
        /// 没有权限
        /// </summary>
        [Description("没有权限")]
        NoAuth = 3005,
        /// <summary>
        /// 上级不存在
        /// </summary>
        [Description("上级不存在")]
        ParentNotExist = 3006,
        /// <summary>
        /// 组织结构不存在
        /// </summary>
        [Description("组织结构不存在")]
        OrganizationUnitNotExist = 3007,
        /// <summary>
        /// 角色不存在
        /// </summary>
        [Description("角色不存在")]
        RoleNotExist = 3008,
        /// <summary>
        /// API配置错误
        /// </summary>
        [Description("API配置错误")]
        ApiHostError = 3009,
        /// <summary>
        /// 系统角色无法操作
        /// </summary>
        [Description("系统角色无法操作")]
        RoleNotOperate = 3010,
        /// <summary>
        /// 系统账号无法操作
        /// </summary>
        [Description("系统账号无法操作")]
        AccountNotOperate = 3011,
        /// <summary>
        /// 非法操作
        /// </summary>
        [Description("非法操作")]
        OperateNotSafe = 3013,
        /// <summary>
        /// 成员已存在
        /// </summary>
        [Description("成员已存在")]
        MemberIsExist = 3014,
        /// <summary>
        /// 姓名重复
        /// </summary>
        [Description("姓名重复")]
        NameIsExist = 3015,
        /// <summary>
        /// 部门已存在
        /// </summary>
        [Description("部门已存在")]
        OrganizationUnitIsExist = 3016,
        /// <summary>
        /// 角色已存在
        /// </summary>
        [Description("角色已存在")]
        RoleIsExist = 3017,
        /// <summary>
        /// 文件类型不符
        /// </summary>
        [Description("文件类型不符")]
        FileExtError = 3018,
        /// <summary>
        /// 只支持单个文件上传
        /// </summary>
        [Description("只支持单个文件上传")]
        FileSingle = 3019,
        /// <summary>
        /// 账号或密码不正确
        /// </summary>
        [Description("账号或密码不正确")]
        AccountOrPasswordError = 3020,
        /// <summary>
        /// 用户使用该角色中,无法删除
        /// </summary>
        [Description("用户使用该角色中,无法删除")]
        AccountUseRole = 3021,
        /// <summary>
        /// 请选择文件
        /// </summary>
        [Description("请选择文件")]
        NoUploadFile = 3022,
        /// <summary>
        /// 请选择角色
        /// </summary>
        [Description("请选择角色")]
        RoleNotSelect = 3023,
        /// <summary>
        /// 手机号错误
        /// </summary>
        [Description("手机号错误")]
        PhoneError = 3024,
        /// <summary>
        /// 员工编号不存在
        /// </summary>
        [Description("员工编号不存在")]
        NumberNotExist = 3025,

        #endregion


    }
}
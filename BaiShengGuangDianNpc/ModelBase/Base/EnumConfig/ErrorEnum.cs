using System.ComponentModel;

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
        /// <summary>
        /// 请求超时
        /// </summary>
        [Description("请求超时")]
        TimeOut,
        /// <summary>
        /// 接口不存在
        /// </summary>
        [Description("接口不存在")]
        ApiDelete,
        /// <summary>
        /// Gate服务器异常
        /// </summary>
        [Description("Gate服务器异常")]
        GateExceptionHappen,
        #region 设备相关 999 - 1999
        /// <summary>
        /// 设备机台号不能为空
        /// </summary>
        [Description("设备机台号不能为空")]
        DeviceCodeNotEmpty = 999,
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
        /// 常用变量不存在
        /// </summary>
        [Description("常用变量不存在")]
        UsuallyDictionaryNotExist = 1053,
        /// <summary>
        /// 常用变量已存在
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
        /// 相同规格、单价、位置的货品编号已存在
        /// </summary>
        [Description("相同规格、单价、位置的货品编号已存在")]
        MaterialBillSpecificationPriceSiteIsExist = 1112,
        /// <summary>
        /// 相同规格、单价、位置的货品编号重复
        /// </summary>
        [Description("相同规格、单价、位置的货品编号重复")]
        MaterialBillSpecificationPriceSiteDuplicate = 1113,

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
        /// 计划已存在
        /// </summary>
        [Description("计划已存在")]
        ManufacturePlanIsExist = 1147,

        /// <summary>
        /// 计划不存在
        /// </summary>
        [Description("计划不存在")]
        ManufacturePlanNotExist = 1148,

        /// <summary>
        /// 计划任务已下发
        /// </summary>
        [Description("计划任务已下发")]
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

        /// <summary>
        /// 变量地址超上限
        /// </summary>
        [Description("变量地址超上限")]
        ValuePointerAddressOutLimit = 1178,
        /// <summary>
        /// 输入地址超上限
        /// </summary>
        [Description("输入地址超上限")]
        InputPointerAddressOutLimit = 1179,
        /// <summary>
        /// 输出地址超上限
        /// </summary>
        [Description("输出地址超上限")]
        OutputPointerAddressOutLimit = 1180,
        /// <summary>
        /// 非等待中设备无法升级
        /// </summary>
        [Description("非等待中设备无法升级")]
        UpgradeDeviceStateError = 1181,
        /// <summary>
        /// 进入流程脚本升级状态失败
        /// </summary>
        [Description("进入流程脚本升级状态失败")]
        UpgradeScriptStateError = 1182,
        /// <summary>
        /// 流程脚本数据发送失败
        /// </summary>
        [Description("流程脚本数据发送失败")]
        UpgradeScriptError = 1183,
        /// <summary>
        /// 升级设备重复
        /// </summary>
        [Description("升级设备重复")]
        UpgradeDeviceDuplicate = 1184,

        /// <summary>
        /// 任务顺序错误
        /// </summary>
        [Description("任务顺序错误")]
        ManufactureTaskItemOrderError = 1185,
        /// <summary>
        /// 进入固件升级状态失败
        /// </summary>
        [Description("进入固件升级状态失败")]
        UpgradeFirmwareStateError = 1186,
        /// <summary>
        /// 固件数据发送失败
        /// </summary>
        [Description("固件数据发送失败")]
        UpgradeFirmwareError = 1187,

        /// <summary>
        /// 需先下发或同时下发关联任务
        /// </summary>
        [Description("需先下发或同时下发关联任务")]
        ManufactureTaskItemAssignAfterRelation = 1188,

        /// <summary>
        /// 无法上移至已下发任务前
        /// </summary>
        [Description("无法上移至已下发任务前")]
        ManufacturePlaneTaskAfterAssign = 1189,
        /// <summary>
        /// 看板设置已存在
        /// </summary>
        [Description("看板设置已存在")]
        MonitoringKanBanSetIsExist = 1190,
        /// <summary>
        /// 看板设置不存在
        /// </summary>
        [Description("看板设置不存在")]
        MonitoringKanBanSetNotExist = 1191,

        /// <summary>
        /// 请购部门已存在
        /// </summary>
        [Description("请购部门已存在")]
        MaterialDepartmentIsExist = 1192,
        /// <summary>
        /// 请购部门不存在
        /// </summary>
        [Description("请购部门不存在")]
        MaterialDepartmentNotExist = 1193,
        /// <summary>
        /// 请购部门名不能为空
        /// </summary>
        [Description("请购部门名不能为空")]
        MaterialDepartmentNotEmpty = 1194,
        /// <summary>
        /// 请购部门名重复
        /// </summary>
        [Description("请购部门名重复")]
        MaterialDepartmentDuplicate = 1195,

        /// <summary>
        /// 请购单已存在
        /// </summary>
        [Description("请购单已存在")]
        MaterialPurchaseIsExist = 1196,
        /// <summary>
        /// 请购单不存在
        /// </summary>
        [Description("请购单不存在")]
        MaterialPurchaseNotExist = 1197,
        /// <summary>
        /// 请购单不能为空
        /// </summary>
        [Description("请购单不能为空")]
        MaterialPurchaseNotEmpty = 1198,
        /// <summary>
        /// 请购单重复
        /// </summary>
        [Description("请购单重复")]
        MaterialPurchaseDuplicate = 1199,

        /// <summary>
        /// 部门员工已存在
        /// </summary>
        [Description("部门员工已存在")]
        MaterialDepartmentMemberIsExist = 1200,
        /// <summary>
        /// 部门员工不存在
        /// </summary>
        [Description("部门员工不存在")]
        MaterialDepartmentMemberNotExist = 1201,
        /// <summary>
        /// 部门员工不能为空
        /// </summary>
        [Description("部门员工不能为空")]
        MaterialDepartmentMemberNotEmpty = 1202,
        /// <summary>
        /// 部门员工重复
        /// </summary>
        [Description("部门员工重复")]
        MaterialDepartmentMemberDuplicate = 1203,

        /// <summary>
        /// 请购物料已存在
        /// </summary>
        [Description("请购物料已存在")]
        MaterialPurchaseItemIsExist = 1204,
        /// <summary>
        /// 请购物料不存在
        /// </summary>
        [Description("请购物料不存在")]
        MaterialPurchaseItemNotExist = 1205,
        /// <summary>
        /// 请购物料不能为空
        /// </summary>
        [Description("请购物料编码不能为空")]
        MaterialPurchaseItemCodeNotEmpty = 1206,
        /// <summary>
        /// 请购物料重复
        /// </summary>
        [Description("请购物料重复")]
        MaterialPurchaseItemDuplicate = 1207,

        /// <summary>
        /// 核价人已存在
        /// </summary>
        [Description("核价人已存在")]
        MaterialValuerIsExist = 1208,
        /// <summary>
        /// 核价人不存在
        /// </summary>
        [Description("核价人不存在")]
        MaterialValuerNotExist = 1209,
        /// <summary>
        /// 核价人不能为空
        /// </summary>
        [Description("核价人不能为空")]
        MaterialValuerNotEmpty = 1210,
        /// <summary>
        /// 请购单非完成状态不能修改
        /// </summary>
        [Description("请购单非完成状态不能修改")]
        MaterialPurchaseSateError = 1211,
        /// <summary>
        /// 当前请购单状态不能入库
        /// </summary>
        [Description("当前请购单状态不能入库")]
        MaterialPurchaseIncreaseSateError = 1212,
        /// <summary>
        /// 物料未开始采购
        /// </summary>
        [Description("物料未开始采购")]
        MaterialPurchaseItemNotBuy = 1213,
        /// <summary>
        /// 设备分类不存在
        /// </summary>
        [Description("设备分类不存在")]
        DeviceClassNotExist = 1214,
        /// <summary>
        /// 设备分类已存在
        /// </summary>
        [Description("设备分类已存在")]
        DeviceClassIsExist = 1215,
        /// <summary>
        /// 设备分类不能为空
        /// </summary>
        [Description("设备分类不能为空")]
        DeviceClassNotEmpty = 1216,
        /// <summary>
        /// 设备使用该分类中,无法删除
        /// </summary>
        [Description("设备使用该分类中,无法删除")]
        DeviceModelUseDeviceClass = 1217,
        /// <summary>
        /// 入库单已存在
        /// </summary>
        [Description("入库单已存在")]
        MaterialPurchaseQuoteIsExist = 1218,
        /// <summary>
        /// 入库单不存在
        /// </summary>
        [Description("入库单不存在")]
        MaterialPurchaseQuoteNotExist = 1219,
        /// <summary>
        /// 信息不能为空
        /// </summary>
        [Description("信息不能为空")]
        MaterialPurchaseQuoteNotEmpty = 1220,
        /// <summary>
        /// 预警设置已存在
        /// </summary>
        [Description("预警设置已存在")]
        WarningSetIsExist = 1221,
        /// <summary>
        /// 预警设置不存在
        /// </summary>
        [Description("预警设置不存在")]
        WarningSetNotExist = 1222,
        /// <summary>
        /// 预警名称不能为空
        /// </summary>
        [Description("预警名称不能为空")]
        WarningSetNotEmpty = 1223,
        /// <summary>
        /// 预警设置项已存在
        /// </summary>
        [Description("预警设置项已存在")]
        WarningSetItemIsExist = 1224,
        /// <summary>
        /// 预警设置项不存在
        /// </summary>
        [Description("预警设置项不存在")]
        WarningSetItemNotExist = 1225,
        /// <summary>
        /// 预警设置项不能为空
        /// </summary>
        [Description("预警设置项不能为空")]
        WarningSetItemNotEmpty = 1226,
        /// <summary>
        /// 请选择设备数据预警
        /// </summary>
        [Description("请选择设备数据预警")]
        WarningSetItemDataTypeError = 1227,
        /// <summary>
        /// 条件设置错误
        /// </summary>
        [Description("条件设置错误")]
        WarningSetItemConditionError = 1228,
        /// <summary>
        /// 频率设置错误
        /// </summary>
        [Description("频率设置错误")]
        WarningSetItemFrequencyError = 1229,
        /// <summary>
        /// 日志不存在
        /// </summary>
        [Description("日志不存在")]
        MaterialLogNotExist = 1230,
        /// <summary>
        /// 后续库存领用数量异常
        /// </summary>
        [Description("后续领用数量异常")]
        MaterialLogConsumeLaterError = 1231,
        /// <summary>
        /// 日志类型必须一致
        /// </summary>
        [Description("日志类型必须一致")]
        MaterialLogTypeDifferent = 1232,
        /// <summary>
        /// 数量不能为负数
        /// </summary>
        [Description("数量不能为负数")]
        NumberCannotBeNegative = 1233,
        /// <summary>
        /// 非本周排班
        /// </summary>
        [Description("非本周排班")]
        OldMaintainerSchedule = 1234,



        /// <summary>
        /// 计划相同
        /// </summary>
        [Description("计划相同")]
        ProductionPlanSame = 1235,

        /// <summary>
        /// 显示条数设置错误
        /// </summary>
        [Description("显示条数设置错误")]
        MonitoringKanBanSetLengthError = 1236,
        /// <summary>
        /// 变量等设置错误
        /// </summary>
        [Description("变量等设置错误")]
        MonitoringKanBanSetVariableError = 1237,

        /// <summary>
        /// 行数设置错误
        /// </summary>
        [Description("行数设置错误")]
        MonitoringKanBanSetRowError = 1238,
        /// <summary>
        /// 列数设置错误
        /// </summary>
        [Description("列数设置错误")]
        MonitoringKanBanSetColError = 1239,
        /// <summary>
        /// 刷新时间设置错误
        /// </summary>
        [Description("刷新时间设置错误")]
        MonitoringKanBanSetSecondError = 1240,
        /// <summary>
        /// 变量顺序重复
        /// </summary>
        [Description("变量顺序重复")]
        MonitoringKanBanSetVariableOrderDuplicate = 1241,
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
        /// 文件不存在
        /// </summary>
        [Description("文件不存在")]
        FileNotExist = 3019,
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
        /// <summary>
        /// 只支持单文件上传
        /// </summary>
        [Description("只支持单文件上传")]
        FileSingle = 3026,
        /// <summary>
        /// 权限已存在
        /// </summary>
        [Description("权限已存在")]
        PermissionIsExist = 3027,
        /// <summary>
        /// 权限不存在
        /// </summary>
        [Description("权限不存在")]
        PermissionNotExist = 3028,
        /// <summary>
        /// 权限名称不能为空
        /// </summary>
        [Description("权限名称不能为空")]
        PermissionNotEmpty = 3029,
        /// <summary>
        /// 权限重复
        /// </summary>
        [Description("权限重复")]
        PermissionDuplicate = 3030,
        #endregion

        #region 智慧工厂 10000-19999

        /// <summary>
        /// 人员不存在
        /// </summary>
        [Description("人员不存在")]
        SmartUserNotExist = 10000,
        /// <summary>
        /// 人员已存在
        /// </summary>
        [Description("人员已存在")]
        SmartUserIsExist = 10001,
        /// <summary>
        /// 人员重复
        /// </summary>
        [Description("人员重复")]
        SmartUserDuplicate = 10002,
        /// <summary>
        /// 人员不能为空
        /// </summary>
        [Description("人员不能为空")]
        SmartUserNotEmpty = 10003,

        /// <summary>
        /// 设备不存在
        /// </summary>
        [Description("设备不存在")]
        SmartDeviceNotExist = 10004,
        /// <summary>
        /// 设备已存在
        /// </summary>
        [Description("设备已存在")]
        SmartDeviceIsExist = 10005,
        /// <summary>
        /// 设备重复
        /// </summary>
        [Description("设备重复")]
        SmartDeviceDuplicate = 10006,
        /// <summary>
        /// 设备不能为空
        /// </summary>
        [Description("设备不能为空")]
        SmartDeviceNotEmpty = 10007,

        /// <summary>
        /// 设备类型不存在
        /// </summary>
        [Description("设备类型不存在")]
        SmartDeviceCategoryNotExist = 10008,
        /// <summary>
        /// 设备已存在
        /// </summary>
        [Description("设备类型已存在")]
        SmartDeviceCategoryIsExist = 10009,
        /// <summary>
        /// 设备类型重复
        /// </summary>
        [Description("设备类型重复")]
        SmartDeviceCategoryDuplicate = 10010,
        /// <summary>
        /// 设备类型不能为空
        /// </summary>
        [Description("设备类型不能为空")]
        SmartDeviceCategoryNotEmpty = 10011,

        /// <summary>
        /// 计划号不存在
        /// </summary>
        [Description("计划号不存在")]
        SmartProductNotExist = 10012,
        /// <summary>
        /// 计划号已存在
        /// </summary>
        [Description("计划号已存在")]
        SmartProductIsExist = 10013,
        /// <summary>
        /// 计划号重复
        /// </summary>
        [Description("计划号重复")]
        SmartProductDuplicate = 10014,
        /// <summary>
        /// 计划号不能为空
        /// </summary>
        [Description("计划号不能为空")]
        SmartProductNotEmpty = 10015,

        /// <summary>
        /// 流程不存在
        /// </summary>
        [Description("流程不存在")]
        SmartProcessNotExist = 10016,
        /// <summary>
        /// 流程已存在
        /// </summary>
        [Description("流程已存在")]
        SmartProcessIsExist = 10017,
        /// <summary>
        /// 流程重复
        /// </summary>
        [Description("流程重复")]
        SmartProcessDuplicate = 10018,
        /// <summary>
        /// 流程不能为空
        /// </summary>
        [Description("流程不能为空")]
        SmartProcessNotEmpty = 10019,

        /// <summary>
        /// 流程编号类型不存在
        /// </summary>
        [Description("流程编号类型不存在")]
        SmartProcessCodeCategoryNotExist = 10020,
        /// <summary>
        /// 流程编号类型已存在
        /// </summary>
        [Description("流程编号类型已存在")]
        SmartProcessCodeCategoryIsExist = 10021,
        /// <summary>
        /// 流程编号类型重复
        /// </summary>
        [Description("流程编号类型重复")]
        SmartProcessCodeCategoryDuplicate = 10022,
        /// <summary>
        /// 流程编号类型不能为空
        /// </summary>
        [Description("流程编号类型不能为空")]
        SmartProcessCodeCategoryNotEmpty = 10023,

        /// <summary>
        /// 流程编号不存在
        /// </summary>
        [Description("流程编号不存在")]
        SmartProcessCodeNotExist = 10024,
        /// <summary>
        /// 流程编号已存在
        /// </summary>
        [Description("流程编号已存在")]
        SmartProcessCodeIsExist = 10025,
        /// <summary>
        /// 流程编号重复
        /// </summary>
        [Description("流程编号重复")]
        SmartProcessCodeDuplicate = 10026,
        /// <summary>
        /// 流程编号不能为空
        /// </summary>
        [Description("流程编号不能为空")]
        SmartProcessCodeNotEmpty = 10027,

        /// <summary>
        /// 工单不存在
        /// </summary>
        [Description("工单不存在")]
        SmartWorkOrderNotExist = 10028,
        /// <summary>
        /// 工单已存在
        /// </summary>
        [Description("工单已存在")]
        SmartWorkOrderIsExist = 10029,
        /// <summary>
        /// 工单重复
        /// </summary>
        [Description("工单重复")]
        SmartWorkOrderDuplicate = 10030,
        /// <summary>
        /// 工单不能为空
        /// </summary>
        [Description("工单不能为空")]
        SmartWorkOrderNotEmpty = 10031,

        /// <summary>
        /// 任务单不存在
        /// </summary>
        [Description("任务单不存在")]
        SmartTaskOrderNotExist = 10032,
        /// <summary>
        /// 任务单已存在
        /// </summary>
        [Description("任务单已存在")]
        SmartTaskOrderIsExist = 10033,
        /// <summary>
        /// 任务单重复
        /// </summary>
        [Description("任务单重复")]
        SmartTaskOrderDuplicate = 10034,
        /// <summary>
        /// 任务单不能为空
        /// </summary>
        [Description("任务单不能为空")]
        SmartTaskOrderNotEmpty = 10035,

        /// <summary>
        /// 流程卡不存在
        /// </summary>
        [Description("流程卡不存在")]
        SmartFlowCardNotExist = 10036,
        /// <summary>
        /// 流程卡已存在
        /// </summary>
        [Description("流程卡已存在")]
        SmartFlowCardIsExist = 10037,
        /// <summary>
        /// 流程卡重复
        /// </summary>
        [Description("流程卡重复")]
        SmartFlowCardDuplicate = 10038,
        /// <summary>
        /// 流程卡不能为空
        /// </summary>
        [Description("流程卡不能为空")]
        SmartFlowCardNotEmpty = 10039,
        /// <summary>
        /// 流程卡加工数量错误
        /// </summary>
        [Description("流程卡加工数量错误")]
        SmartFlowCardNumberError = 10040,
        /// <summary>
        /// 产量已达标
        /// </summary>
        [Description("产量已达标")]
        SmartFlowCardNumberLimit = 10041,

        /// <summary>
        /// 标准流程不存在
        /// </summary>
        [Description("标准流程不存在")]
        SmartProcessCodeCategoryProcessNotExist = 10042,
        /// <summary>
        /// 标准流程已存在
        /// </summary>
        [Description("标准流程已存在")]
        SmartProcessCodeCategoryProcessIsExist = 10043,
        /// <summary>
        /// 标准流程重复
        /// </summary>
        [Description("标准流程重复")]
        SmartProcessCodeCategoryProcessDuplicate = 10044,
        /// <summary>
        /// 标准流程不能为空
        /// </summary>
        [Description("标准流程不能为空")]
        SmartProcessCodeCategoryProcessNotEmpty = 10045,

        /// <summary>
        /// 工序不存在
        /// </summary>
        [Description("工序不存在")]
        SmartFlowCardProcessNotExist = 10046,
        /// <summary>
        /// 工序已存在
        /// </summary>
        [Description("工序已存在")]
        SmartFlowCardProcessIsExist = 10047,
        /// <summary>
        /// 工序重复
        /// </summary>
        [Description("工序重复")]
        SmartFlowCardProcessDuplicate = 10048,
        /// <summary>
        /// 工序不能为空
        /// </summary>
        [Description("工序不能为空")]
        SmartFlowCardProcessNotEmpty = 10049,

        /// <summary>
        /// 流程编号类型应相同
        /// </summary>
        [Description("流程编号类型应相同")]
        SmartProductProcessCodeCategoryMustBeSame = 10050,

        /// <summary>
        /// 工序异常不存在
        /// </summary>
        [Description("工序异常不存在")]
        SmartProcessFaultNotExist = 10051,
        /// <summary>
        /// 工序异常已存在
        /// </summary>
        [Description("工序异常已存在")]
        SmartProcessFaultIsExist = 10052,

        /// <summary>
        /// 操作工不存在
        /// </summary>
        [Description("操作工不存在")]
        SmartOperatorNotExist = 10053,
        /// <summary>
        /// 操作工已存在
        /// </summary>
        [Description("操作工已存在")]
        SmartOperatorIsExist = 10054,
        /// <summary>
        /// 操作工重复
        /// </summary>
        [Description("操作工重复")]
        SmartOperatorDuplicate = 10055,
        /// <summary>
        /// 操作工不能为空
        /// </summary>
        [Description("操作工不能为空")]
        SmartOperatorNotEmpty = 10056,

        /// <summary>
        /// 员工等级不存在
        /// </summary>
        [Description("员工等级不存在")]
        SmartOperatorLevelNotExist = 10057,
        /// <summary>
        /// 员工等级已存在
        /// </summary>
        [Description("员工等级已存在")]
        SmartOperatorLevelIsExist = 10058,
        /// <summary>
        /// 员工等级重复
        /// </summary>
        [Description("员工等级重复")]
        SmartOperatorLevelDuplicate = 10059,
        /// <summary>
        /// 员工等级不能为空
        /// </summary>
        [Description("员工等级不能为空")]
        SmartOperatorLevelNotEmpty = 10060,

        /// <summary>
        /// 设备型号不存在
        /// </summary>
        [Description("设备型号不存在")]
        SmartDeviceModelNotExist = 10061,
        /// <summary>
        /// 设备型号已存在
        /// </summary>
        [Description("设备型号已存在")]
        SmartDeviceModelIsExist = 10062,
        /// <summary>
        /// 设备型号重复
        /// </summary>
        [Description("设备型号重复")]
        SmartDeviceModelDuplicate = 10063,
        /// <summary>
        /// 设备型号不能为空
        /// </summary>
        [Description("设备型号不能为空")]
        SmartDeviceModelNotEmpty = 10064,

        /// <summary>
        /// 产能类型不存在
        /// </summary>
        [Description("产能配置不存在")]
        SmartCapacityNotExist = 10065,
        /// <summary>
        /// 产能配置已存在
        /// </summary>
        [Description("产能配置已存在")]
        SmartCapacityIsExist = 10066,
        /// <summary>
        /// 产能配置重复
        /// </summary>
        [Description("产能配置重复")]
        SmartCapacityDuplicate = 10067,
        /// <summary>
        /// 产能配置不能为空
        /// </summary>
        [Description("产能配置不能为空")]
        SmartCapacityNotEmpty = 10068,

        /// <summary>
        /// 产能配置项不存在
        /// </summary>
        [Description("产能配置项不存在")]
        SmartCapacityListNotExist = 10069,
        /// <summary>
        /// 产能配置项已存在
        /// </summary>
        [Description("产能配置项已存在")]
        SmartCapacityListIsExist = 10070,
        /// <summary>
        /// 产能配置项重复
        /// </summary>
        [Description("产能配置项重复")]
        SmartCapacityListDuplicate = 10071,
        /// <summary>
        /// 产能配置项不能为空
        /// </summary>
        [Description("产能配置项不能为空")]
        SmartCapacityListNotEmpty = 10072,
        /// <summary>
        /// 设置数量与标准流程数量不符
        /// </summary>
        [Description("设置数量与标准流程数量不符")]
        SmartCapacityListCountError = 10073,

        /// <summary>
        /// 计划号产能不存在
        /// </summary>
        [Description("计划号产能不存在")]
        SmartProductCapacityNotExist = 10074,
        /// <summary>
        /// 计划号产能已存在
        /// </summary>
        [Description("计划号产能已存在")]
        SmartProductCapacityIsExist = 10075,
        /// <summary>
        /// 计划号产能重复
        /// </summary>
        [Description("计划号产能重复")]
        SmartProductCapacityDuplicate = 10076,
        /// <summary>
        /// 计划号产能不能为空
        /// </summary>
        [Description("计划号产能不能为空")]
        SmartProductCapacityNotEmpty = 10077,

        /// <summary>
        /// 任务单等级不存在
        /// </summary>
        [Description("任务单等级不存在")]
        SmartTaskOrderLevelNotExist = 10078,
        /// <summary>
        /// 任务单等级已存在
        /// </summary>
        [Description("任务单等级已存在")]
        SmartTaskOrderLevelIsExist = 10079,
        /// <summary>
        /// 任务单等级重复
        /// </summary>
        [Description("任务单等级重复")]
        SmartTaskOrderLevelDuplicate = 10080,
        /// <summary>
        /// 任务单等级不能为空
        /// </summary>
        [Description("任务单等级不能为空")]
        SmartTaskOrderLevelNotEmpty = 10081,

        /// <summary>
        /// 任务单重复排产
        /// </summary>
        [Description("任务单重复排产")]
        SmartTaskOrderArranged = 10082,
        /// <summary>
        /// 任务单未排产
        /// </summary>
        [Description("任务单未排产")]
        SmartTaskOrderNotArranged = 10083,

        /// <summary>
        /// 已有数量不应大于目标量
        /// </summary>
        [Description("已有数量不应大于目标量")]
        SmartTaskOrderNeedStockLarge = 10084,
        /// <summary>
        /// 工序产能未设置
        /// </summary>
        [Description("工序产能未设置")]
        SmartCapacityListNotSet = 10085,
        /// <summary>
        /// 合格率错误
        /// </summary>
        [Description("合格率错误")]
        SmartCapacityRateError = 10086,

        /// <summary>
        /// 任务单缺少工序
        /// </summary>
        [Description("任务单缺少工序")]
        SmartScheduleNeedLost = 10087,

        /// <summary>
        /// 车间不存在
        /// </summary>
        [Description("车间不存在")]
        SmartWorkshopNotExist = 10090,
        /// <summary>
        /// 车间已存在
        /// </summary>
        [Description("车间已存在")]
        SmartWorkshopIsExist = 10091,
        /// <summary>
        /// 车间重复
        /// </summary>
        [Description("车间重复")]
        SmartWorkshopDuplicate = 10092,
        /// <summary>
        /// 车间不能为空
        /// </summary>
        [Description("车间不能为空")]
        SmartWorkshopNotEmpty = 10093,
        /// <summary>
        /// 频率必须大于零
        /// </summary>
        [Description("频率必须大于零")]
        SmartWorkshopFrequency0 = 10094,
        #endregion



    }
}
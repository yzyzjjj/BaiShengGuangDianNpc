using System.Collections.Generic;

namespace ModelBase.Base.UrlMappings
{
    public class UrlMappings
    {
        public static string deviceList = "deviceList";
        public static string addDevice = "addDevice";
        public static string batchAddDevice = "batchAddDevice";
        public static string delDevice = "delDevice";
        public static string batchDelDevice = "batchDelDevice";
        public static string batchUpdateDevice = "batchUpdateDevice";
        public static string send = "send";
        public static string batchSend = "batchSend";
        public static string sendBack = "sendBack";
        public static string batchSendBack = "batchSendBack";
        public static string setStorage = "setStorage";
        public static string batchSetStorage = "batchSetStorage";
        public static string setFrequency = "setFrequency";
        public static string batchSetFrequency = "batchSetFrequency";
        public static string batchUpgrade = "batchUpgrade";

        public static string deviceListGate = "deviceListGate";
        public static string deviceSingleGate = "deviceSingleGate";
        public static string batchAddDeviceGate = "batchAddDeviceGate";
        public static string batchDelDeviceGate = "batchDelDeviceGate";
        public static string batchUpdateDeviceGate = "batchUpdateDeviceGate";
        public static string batchSendBackGate = "batchSendBackGate";
        public static string batchSetStorageGate = "batchSetStorageGate";
        public static string batchSetFrequencyGate = "batchSetFrequencyGate";
        public static string batchUpgradeGate = "batchUpgradeGate";

        public static Dictionary<string, string> Urls = new Dictionary<string, string>
        {
            { deviceList,"/npc/device/list"},
            { addDevice,"/npc/device/add"},
            { batchAddDevice,"/npc/device/batchadd"},
            { delDevice,"/npc/device/delete"},
            { batchDelDevice,"/npc/device/batchdelete"},
            { batchUpdateDevice,"/npc/device/batchupdate"},
            { send,"/npc/device/send"},
            { batchSend,"/npc/device/batchsend"},
            { sendBack,"/npc/device/sendback"},
            { batchSendBack,"/npc/device/batchsendback"},
            { setStorage,"/npc/device/setstorage"},
            { batchSetStorage,"/npc/device/batchsetstorage"},
            { setFrequency,"/npc/device/setfrequency"},
            { batchSetFrequency,"/npc/device/batchsetfrequency"},
            { batchUpgrade,"/npc/device/batchUpgrade"},

            { deviceListGate,"/gate/device/list"},
            { deviceSingleGate,"/gate/device/single"},
            { batchAddDeviceGate,"/gate/device/batchadd"},
            { batchDelDeviceGate,"/gate/device/batchdelete"},
            { batchUpdateDeviceGate,"/gate/device/batchupdate"},
            { batchSendBackGate,"/gate/device/batchsendback"},
            { batchSetStorageGate,"/gate/device/batchsetstorage"},
            { batchSetFrequencyGate,"/gate/device/batchsetfrequency"},
            { batchUpgradeGate,"/gate/device/batchUpgrade"},
        };
    }
}

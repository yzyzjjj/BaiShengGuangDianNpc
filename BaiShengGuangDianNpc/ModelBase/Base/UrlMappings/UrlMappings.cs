using System.Collections.Generic;

namespace ModelBase.Base.UrlMappings
{
    public class UrlMappings
    {
        public static Dictionary<string, string> Urls = new Dictionary<string, string>
        {
            { "deviceList", "/npc/device/list"},
            { "addDevice", "/npc/device/add"},
            { "batchAddDevice", "/npc/device/batchadd"},
            { "delDevice", "/npc/device/delete"},
            { "batchDelDevice", "/npc/device/batchdelete"},
            { "batchUpdateDevice", "/npc/device/batchupdate"},
            { "send", "/npc/device/send"},
            { "batchSend", "/npc/device/batchsend"},
            { "sendBack", "/npc/device/sendback"},
            { "batchSendBack", "/npc/device/batchsendback"},
            { "setStorage", "/npc/device/setstorage"},
            { "batchSetStorage", "/npc/device/batchsetstorage"},
            { "setFrequency", "/npc/device/setfrequency"},
            { "batchSetFrequency", "/npc/device/batchsetfrequency"},

            { "deviceListGate", "/gate/device/list"},
            { "deviceSingleGate", "/gate/device/single"},
            { "batchAddDeviceGate", "/gate/device/batchadd"},
            { "batchDelDeviceGate", "/gate/device/batchdelete"},
            { "batchUpdateDeviceGate", "/gate/device/batchupdate"},
            { "batchSendBackGate", "/gate/device/batchsendback"},
            { "batchSetStorageGate", "/gate/device/batchsetstorage"},
            { "batchSetFrequencyGate", "/gate/device/batchsetfrequency"},
        };
    }
}

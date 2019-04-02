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
            { "addDeviceGate", "/gate/device/add"},
            { "batchAddDeviceGate", "/gate/device/batchadd"},
            { "delDeviceGate", "/gate/device/delete"},
            { "batchDelDeviceGate", "/gate/device/batchdelete"},
            { "sendGate", "/gate/device/send"},
            { "batchSendGate", "/gate/device/batchsend"},
            { "sendBackGate", "/gate/device/sendback"},
            { "batchSendBackGate", "/gate/device/batchsendback"},
            { "setStorageGate", "/gate/device/setstorage"},
            { "batchSetStorageGate", "/gate/device/batchsetstorage"},
            { "setFrequencyGate", "/gate/device/setfrequency"},
            { "batchSetFrequencyGate", "/gate/device/batchsetfrequency"},
        };
    }
}

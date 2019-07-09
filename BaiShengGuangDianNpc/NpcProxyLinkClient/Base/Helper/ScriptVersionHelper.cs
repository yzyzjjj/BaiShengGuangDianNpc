using System.Collections.Generic;
using System.Linq;
using NpcProxyLink.Models;
using NpcProxyLinkClient.Base.Server;

namespace NpcProxyLinkClient.Base.Helper
{
    /// <summary>
    /// 脚本类
    /// </summary>
    public class ScriptVersionHelper
    {
        public static IEnumerable<ScriptVersion> Datas;

        public static string TableName = "script_version";

        public static void LoadConfig()
        {
            Datas = ServerConfig.ApiDb.Query<ScriptVersion>("SELECT * FROM `script_version` WHERE MarkedDelete = 0;");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scriptId"></param>
        /// <returns></returns>
        public static ScriptVersion Get(int scriptId)
        {
            return Datas.FirstOrDefault(x => x.Id == scriptId);
        }
    }
}

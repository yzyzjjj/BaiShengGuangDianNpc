using NpcProxyLink.Base.Server;
using NpcProxyLink.Models;
using System.Collections.Generic;
using System.Linq;

namespace NpcProxyLink.Base.Helper
{
    /// <summary>
    /// 常用变量类
    /// </summary>
    public class UsuallyDictionaryHelper
    {
        public static IEnumerable<UsuallyDictionary> Datas;

        public static string TableName = "usually_dictionary";

        public static void LoadConfig()
        {
            Datas = ServerConfig.DeviceDb.Query<UsuallyDictionary>("SELECT * FROM `usually_dictionary`;");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scriptId"></param>
        /// <param name="variableNameId"></param>
        /// <returns></returns>
        public static UsuallyDictionary Get(int scriptId, int variableNameId)
        {
            return Datas.FirstOrDefault(x => x.ScriptId == scriptId && x.VariableNameId == variableNameId);
        }
    }
}

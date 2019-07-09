using System.Linq;
using GateProxyLinkServer.Base.Logic;
using Microsoft.Extensions.Configuration;
using ModelBase.Base.Dapper;
using ModelBase.Base.Logger;
using ModelBase.Base.Utils;

namespace GateProxyLinkServer.Base.Server
{
    public class ServerConfig
    {
        public static DataBase ApiDb;
        public static ServerManager ServerManager;
        public static int ServerId;
        public static void Init(IConfiguration configuration)
        {
            ApiDb = new DataBase(configuration.GetConnectionString("ApiDb"));
            ServerId = configuration.GetAppSettings<int>("ServerId");



            ServerManager = new ServerManager();
            ServerManager.Init();



            Log.InfoFormat("ServerConfig Done");
        }

    }
}

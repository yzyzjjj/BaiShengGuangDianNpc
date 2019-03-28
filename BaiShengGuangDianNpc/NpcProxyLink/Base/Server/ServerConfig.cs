using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ModelBase.Base.Dapper;
using ModelBase.Base.Logger;
using ModelBase.Base.Utils;
using NpcProxyLink.Base.Logic;

namespace NpcProxyLink.Base.Server
{
    public class ServerConfig
    {
        public static DataBase DeviceDb;
        public static DataBase DataStoragDb;
        public static ClientManager ClientManager;
        public static int ServerId;

        public static void Init(IConfiguration configuration)
        {
            DeviceDb = new DataBase(configuration.GetConnectionString("DeviceDb"));
            DataStoragDb = new DataBase(configuration.GetConnectionString("DataStoragDb"));
            ServerId = configuration.GetAppSettings<int>("ServerId");

            ClientManager = new ClientManager();
            ClientManager.LoadConfig();




            Log.InfoFormat("ServerConfig Done, Count:{0}", ClientManager.GetDevices().Count());
        }

    }
}

using Dapper;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ModelBase.Base.Dapper
{
    public class DataBase
    {
        private readonly string _connectionString;
        public DataBase(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<T> Query<T>(string sql, object param = null, int? second = null)
        {
            using (var con = new MySqlConnection(_connectionString))
            {
                return con.Query<T>(sql, param, null, true, second);
            }
        }

        public int Execute(string sql, object param = null, int? second = null)
        {
            using (var con = new MySqlConnection(_connectionString))
            {
                return con.Execute(sql, param, null, second);
            }
        }

        public Task<int> ExecuteAsync(string sql, object param = null, int? second = null)
        {
            using (var con = new MySqlConnection(_connectionString))
            {
                return con.ExecuteAsync(sql, param, null, second);
            }
        }

        public int ExecuteTrans(string sql, object param = null, int? second = null)
        {
            using (var con = new MySqlConnection(_connectionString))
            {
                //return con.ExecuteAsync(sql, param, null, second);
                con.Open();
                var trans = con.BeginTransaction();
                var r = con.Execute(sql, param, trans);
                trans.Commit();
                return r;
            }
        }
    }
}

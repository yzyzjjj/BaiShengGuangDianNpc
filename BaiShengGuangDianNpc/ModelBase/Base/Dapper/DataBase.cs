﻿using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;

namespace ModelBase.Base.Dapper
{
    public class DataBase
    {
        private readonly string _connectionString;
        public DataBase(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<T> Query<T>(string sql, object param = null)
        {
            using (var con = new MySqlConnection(_connectionString))
            {
                return con.Query<T>(sql, param);
            }
        }
        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null)
        {
            using (var con = new MySqlConnection(_connectionString))
            {
                return await con.QueryAsync<T>(sql, param);
            }
        }

        public int Execute(string sql, object param = null)
        {
            using (var con = new MySqlConnection(_connectionString))
            {
                return con.Execute(sql, param);
            }
        }
    }
}

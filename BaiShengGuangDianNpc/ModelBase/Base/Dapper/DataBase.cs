﻿using Dapper;
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

        public IEnumerable<T> Query<T>(string sql, object param = null)
        {
            using (var con = new MySqlConnection(_connectionString))
            {
                return con.Query<T>(sql, param);
            }
        }
        public IEnumerable<T> QueryWithTime<T>(string sql, object param = null, int? second = null)
        {
            if (second == null)
            {
                second = 60;
            }

            using (var con = new MySqlConnection(_connectionString))
            {
                return con.Query<T>(sql, param, null, true, second);
            }
        }

        public int Execute(string sql, object param = null)
        {
            using (var con = new MySqlConnection(_connectionString))
            {
                return con.Execute(sql, param);
            }
        }

        public Task<int> ExecuteAsync(string sql, object param = null)
        {
            using (var con = new MySqlConnection(_connectionString))
            {
                return con.ExecuteAsync(sql, param);
            }
        }
    }
}

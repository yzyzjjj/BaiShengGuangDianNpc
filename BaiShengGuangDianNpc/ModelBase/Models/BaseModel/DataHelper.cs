using ApiManagement.Base.Server;
using Dapper;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using ModelBase.Base.Dapper;
using ModelBase.Models.BaseModel;

namespace ApiManagement.Models.BaseModel
{
    public interface IDataHelper
    {
        T Get<T>(int id, string querySql = "") where T : CommonBase;
        //IEnumerable<T> GetByIds<T>(IEnumerable<int> ids) where T : CommonBase;
        IEnumerable<T> GetByIds<T>(IEnumerable<int> ids, string querySql = "");
        IEnumerable<T> GetAll<T>(string querySql = "") where T : CommonBase;
        IEnumerable<T> GetAllData<T>(string querySql = "") where T : CommonBase;
        int GetCountByIds(IEnumerable<int> ids, string querySql = "");
        int GetCountAll(string querySql = "");
    }
    public abstract class DataHelper : IDataHelper
    {
        public Dictionary<string,string> Op = new Dictionary<string, string>
        {
            {"FIND_IN_SET", "FIND_IN_SET({0}, {1}) AND "}
        };
        #region 字段
        /// <summary>
        /// 表名
        /// </summary>
        protected DataBase DB = null;
        /// <summary>
        /// 表名
        /// </summary>
        protected string Table = "";
        /// <summary>
        /// 判重字段名
        /// </summary>
        protected string SameField = "";
        /// <summary>
        /// 判重组合查询字段
        /// </summary>
        //protected List<string> SameQueryFields = new List<string>();
        /// <summary>
        /// 判重组合查询字段条件
        /// </summary>
        //protected List<string> SameQueryFieldConditions = new List<string>();
        /// <summary>
        /// 菜单字段
        /// </summary>
        protected List<string> MenuFields = new List<string>();
        /// <summary>
        /// 组合查询字段 字段, 符合(条件), 数据
        /// </summary>
        //protected List<Tuple<string, string, dynamic>> QueryFields = new List<Tuple<string, string, dynamic>>();
        /// <summary>
        /// 查询语句
        /// </summary>
        //protected string QuerySql = "";
        /// <summary>
        /// 添加语句
        /// </summary>
        protected string InsertSql = "";
        /// <summary>
        /// 更新语句
        /// </summary>
        protected string UpdateSql = "";
        /// <summary>
        /// 根据父级删除
        /// </summary>
        protected string ParentField = "";
        //protected string DeleteSql = "";
        #endregion

        #region IDataHelper
        /// <summary>
        /// 单个获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="querySql"></param>
        /// <returns></returns>
        public T Get<T>(int id, string querySql = "") where T : CommonBase
        {
            return DB.Query<T>(!querySql.IsNullOrEmpty() ? querySql : $"SELECT * FROM `{Table}` WHERE `MarkedDelete` = 0 AND Id = @id;", new { id }).FirstOrDefault();
        }
        ///// <summary>
        ///// 通过id批量获取
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="ids"></param>
        ///// <returns></returns>
        //public IEnumerable<T> GetByIds<T>(IEnumerable<int> ids) where T : CommonBase
        //{
        //    if (ids == null || !ids.Any())
        //    {
        //        return new List<T>();
        //    }

        //    return DB.Query<T>($"SELECT * FROM `{Table}` WHERE `MarkedDelete` = 0 AND Id IN @ids;", new { ids });
        //}
        /// <summary>
        /// 通过id批量获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids"></param>
        /// <param name="querySql"></param>
        /// <returns></returns>
        public IEnumerable<T> GetByIds<T>(IEnumerable<int> ids, string querySql = "")
        {
            if (ids == null || !ids.Any())
            {
                return new List<T>();
            }

            return DB.Query<T>(!querySql.IsNullOrEmpty() ? querySql : $"SELECT * FROM `{Table}` WHERE `MarkedDelete` = 0 AND Id IN @ids;", new { ids });
        }
        /// <summary>
        /// 通过id批量获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids"></param>
        /// <param name="querySql"></param>
        /// <returns></returns>
        public IEnumerable<T> GetAllByIds<T>(IEnumerable<int> ids, string querySql = "") where T : CommonBase
        {
            if (ids == null || !ids.Any())
            {
                return new List<T>();
            }

            return DB.Query<T>(!querySql.IsNullOrEmpty() ? querySql : $"SELECT * FROM `{Table}` WHERE Id IN @ids;", new { ids });
        }
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="querySql"></param>
        /// <returns></returns>
        public IEnumerable<T> GetAll<T>(string querySql = "") where T : CommonBase
        {
            return DB.Query<T>(!querySql.IsNullOrEmpty() ? querySql : $"SELECT * FROM `{Table}` WHERE `MarkedDelete` = 0;");
        }
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="querySql"></param>
        /// <returns></returns>
        public IEnumerable<T> GetAllData<T>(string querySql = "") where T : CommonBase
        {
            return DB.Query<T>(!querySql.IsNullOrEmpty() ? querySql : $"SELECT * FROM `{Table}`;");
        }
        /// <summary>
        /// 通过id批量获取数量
        /// </summary>
        /// <param name="id"></param>
        /// <param name="querySql"></param>
        /// <returns></returns>
        public int GetCountById(int id, string querySql = "")
        {
            return DB.Query<int>(!querySql.IsNullOrEmpty() ? querySql : $"SELECT COUNT(1) FROM `{Table}` WHERE `MarkedDelete` = 0 AND Id = @id;", new { id }).FirstOrDefault();
        }
        /// <summary>
        /// 通过id批量获取数量
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="querySql"></param>
        /// <returns></returns>
        public int GetCountByIds(IEnumerable<int> ids, string querySql = "")
        {
            if (ids == null || !ids.Any())
            {
                return 0;
            }

            return DB.Query<int>(!querySql.IsNullOrEmpty() ? querySql : $"SELECT COUNT(1) FROM `{Table}` WHERE `MarkedDelete` = 0 AND Id IN @ids;", new { ids }).FirstOrDefault();
        }
        /// <summary>
        /// 获取总数量
        /// <param name="querySql"></param>
        /// </summary>
        /// <returns></returns>
        public int GetCountAll(string querySql = "")
        {
            return DB.Query<int>(!querySql.IsNullOrEmpty() ? querySql : $"SELECT COUNT(1) FROM `{Table}` WHERE `MarkedDelete` = 0;").FirstOrDefault();
        }

        #endregion

        #region 自定义Get
        /// <summary>
        /// 获取(通用接口)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="menu">菜单</param>
        /// <param name="querySql"></param>
        /// <returns></returns>
        public IEnumerable<T> CommonGet<T>(int id = 0, bool menu = false, string querySql = "")
        {
            return DB.Query<T>(!querySql.IsNullOrEmpty() ? querySql : $"SELECT {(menu && MenuFields.Any() ? MenuFields.Select(x => $"`{x}`").Join(", ") : "*")} FROM `{Table}` WHERE{(id == 0 ? "" : " Id = @id AND ")}`MarkedDelete` = 0;", new { id });
        }
        /// <summary>
        /// 多条件获取(通用接口) MenuFields  MenuQueryFields
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="args">字段, 符合(条件), 数据</param>
        /// <param name="menu">菜单</param>
        /// <returns></returns>
        public IEnumerable<T> CommonGet<T>(IEnumerable<Tuple<string, string, dynamic>> args, bool menu = false)
        {
            var d = new DynamicParameters();
            var param = new List<string>();
            var argsGroup = args.GroupBy(x => x.Item1).ToDictionary(x => x.Key, x => x.Count());
            var argsGroupCount = args.GroupBy(x => x.Item1).ToDictionary(x => x.Key, x => 0);
            foreach (var arg in args)
            {
                var pName = arg.Item1;
                if (argsGroup[arg.Item1] > 1)
                {
                    var index = argsGroupCount[arg.Item1]++;
                    pName = $"{arg.Item1}{index}";
                }

                param.Add(Op.ContainsKey(arg.Item2)
                    ? Op[arg.Item2].FormatWith($"@{pName}", arg.Item1)
                    : $"`{arg.Item1}` {arg.Item2} @{pName} AND ");
                d.Add(pName, arg.Item3);
            }
            return CommonGet<T>((menu && MenuFields.Any() ? MenuFields.Select(x => $"`{x}`").Join(", ") : "*"), param.Join(""), d);
        }
        private IEnumerable<T> CommonGet<T>(string field, string param, DynamicParameters arg)
        {
            return param.IsNullOrEmpty() ? DB.Query<T>($"SELECT {field} FROM `{Table}` WHERE `MarkedDelete` = 0;")
            : DB.Query<T>($"SELECT {field} FROM `{Table}` WHERE {param}`MarkedDelete` = 0;", arg);
        }
        //public IEnumerable<T> CommonGet<T>(IEnumerable<Tuple<string, string, dynamic>> args, bool menu = false)
        //{
        //    var vs = new JObject();
        //    var param = new List<string>();
        //    foreach (var arg in args)
        //    {
        //        param.Add($"{arg.Item1} {arg.Item2} @{arg.Item1} AND ");
        //        vs[arg.Item1] = arg.Item3;
        //    }

        //    return DB.Query<T>($"SELECT {(menu && MenuFields.Any() ? MenuFields.Select(x => $"`{x}`").Join() : "*")} FROM `{Table}` WHERE {param.Join("")}`MarkedDelete` = 0;", vs);
        //}

        /// <summary>
        /// 多条件获取数量(通用接口) MenuFields  MenuQueryFields
        /// </summary>
        /// <param name="args">字段, 符合(条件), 数据</param>
        /// <returns></returns>
        public int CommonGetCount(IEnumerable<Tuple<string, string, dynamic>> args)
        {
            var d = new DynamicParameters();
            var param = new List<string>();
            var argsGroup = args.GroupBy(x => x.Item1).ToDictionary(x => x.Key, x => x.Count());
            var argsGroupCount = args.GroupBy(x => x.Item1).ToDictionary(x => x.Key, x => 0);
            foreach (var arg in args)
            {
                var pName = arg.Item1;
                if (argsGroup[arg.Item1] > 1)
                {
                    var index = argsGroupCount[arg.Item1]++;
                    pName = $"{arg.Item1}{index}";
                }

                param.Add(Op.ContainsKey(arg.Item2)
                    ? Op[arg.Item2].FormatWith($"@{pName}", arg.Item1)
                    : $"`{arg.Item1}` {arg.Item2} @{pName} AND ");
                d.Add(pName, arg.Item3);
            }
            return CommonGetCount(param.Join(""), d);
        }
        private int CommonGetCount(string param, DynamicParameters arg)
        {
            return param.IsNullOrEmpty() ? DB.Query<int>($"SELECT COUNT(1) FROM `{Table}` WHERE `MarkedDelete` = 0;").FirstOrDefault()
                : DB.Query<int>($"SELECT COUNT(1) FROM `{Table}` WHERE {param}`MarkedDelete` = 0;", arg).FirstOrDefault();
        }
        /// <summary>
        /// 获取指定字段重复数据
        /// </summary>
        /// <param name="sames"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IEnumerable<string> GetSames(IEnumerable<string> sames, IEnumerable<int> ids = null)
        {
            return DB.Query<string>($"SELECT `{SameField}` FROM `{Table}` WHERE `{SameField}` IN @sames{(ids != null ? " AND `Id` NOT IN @ids" : "")} AND `MarkedDelete` = 0;", new { sames, ids });
        }
        /// <summary>
        /// 多条件获取指定字段重复数据(通用接口)
        /// </summary>
        /// <param name="args">字段, 符合(条件), 数据</param>
        /// <returns></returns>
        public IEnumerable<string> CommonGetSames(IEnumerable<Tuple<string, string, dynamic>> args)
        {
            var d = new DynamicParameters();
            var param = new List<string>();
            var argsGroup = args.GroupBy(x => x.Item1).ToDictionary(x => x.Key, x => x.Count());
            var argsGroupCount = args.GroupBy(x => x.Item1).ToDictionary(x => x.Key, x => 0);
            foreach (var arg in args)
            {
                var pName = arg.Item1;
                if (argsGroup[arg.Item1] > 1)
                {
                    var index = argsGroupCount[arg.Item1]++;
                    pName = $"{arg.Item1}{index}";
                }
                param.Add(Op.ContainsKey(arg.Item2)
                    ? Op[arg.Item2].FormatWith($"@{pName}", arg.Item1)
                    : $"`{arg.Item1}` {arg.Item2} @{pName} AND ");
                d.Add(arg.Item1, arg.Item3);
            }
            return CommonGetSames<string>(param.Join(""), d);
        }
        private IEnumerable<string> CommonGetSames<T>(string param, DynamicParameters arg)
        {
            return DB.Query<string>($"SELECT {SameField} FROM `{Table}` WHERE {param}`MarkedDelete` = 0;", arg);
        }
        /// <summary>
        /// 获取指定字段重复数据数量（只判断使用HaveSame）
        /// </summary>
        /// <param name="sames"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int GetSameCount(IEnumerable<string> sames, IEnumerable<int> ids = null)
        {
            return DB.Query<int>($"SELECT COUNT(1) FROM `{Table}` WHERE `{SameField}` IN @sames{(ids != null ? " AND `Id` NOT IN @ids" : "")} AND `MarkedDelete` = 0;",
                new { sames, ids }).FirstOrDefault();
        }
        /// <summary>
        /// 多条件获取指定字段重复数据数量（只判断使用HaveSame）
        /// </summary>
        /// <param name="args">字段, 符合(条件), 数据</param>
        /// <returns></returns>
        public int CommonGetSameCount(IEnumerable<Tuple<string, string, dynamic>> args)
        {
            var d = new DynamicParameters();
            var param = new List<string>();
            var argsGroup = args.GroupBy(x => x.Item1).ToDictionary(x => x.Key, x => x.Count());
            var argsGroupCount = args.GroupBy(x => x.Item1).ToDictionary(x => x.Key, x => 0);
            foreach (var arg in args)
            {
                var pName = arg.Item1;
                if (argsGroup[arg.Item1] > 1)
                {
                    var index = argsGroupCount[arg.Item1]++;
                    pName = $"{arg.Item1}{index}";
                }
                param.Add(Op.ContainsKey(arg.Item2)
                    ? Op[arg.Item2].FormatWith($"@{pName}", arg.Item1)
                    : $"`{arg.Item1}` {arg.Item2} @{pName} AND ");
                d.Add(arg.Item1, arg.Item3);
            }
            return CommonGetSameCount(param.Join(""), d);
        }
        private int CommonGetSameCount(string param, DynamicParameters arg)
        {
            return DB.Query<int>($"SELECT COUNT(1) FROM `{Table}` WHERE {param}`MarkedDelete` = 0;", arg).FirstOrDefault();
        }
        /// <summary>
        /// 获取指定字段是否有重复数据
        /// </summary>
        /// <param name="sames"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool HaveSame(IEnumerable<string> sames, IEnumerable<int> ids = null)
        {
            if (SameField.IsNullOrEmpty())
            {
                throw new Exception("DataHelper SameField IsNullOrEmpty!!!");
            }

            return GetSameCount(sames, ids) > 0;
        }

        /// <summary>
        /// 多条件获取指定字段是否有重复数据（只判断使用CommonHaveSame）
        /// </summary>
        /// <param name="args">字段, 符合(条件), 数据</param>
        /// <returns></returns>
        public bool CommonHaveSame(IEnumerable<Tuple<string, string, dynamic>> args)
        {
            if (SameField.IsNullOrEmpty())
            {
                throw new Exception("DataHelper SameField IsNullOrEmpty!!!");
            }

            return CommonGetSameCount(args) > 0;
        }
        #endregion

        #region Add
        /// <summary>
        /// 单个添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public void Add<T>(T t)
        {
            DB.Execute(InsertSql, t);
        }
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public void Add<T>(IEnumerable<T> t)
        {
            DB.Execute(InsertSql, t);
        }

        #endregion

        #region Update
        /// <summary>
        /// 单个更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public void Update<T>(T t) where T : CommonBase
        {
            DB.Execute(UpdateSql, t);
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public void Update<T>(IEnumerable<T> t)
        {
            DB.Execute(UpdateSql, t);
        }
        /// <summary>
        /// 多条件更新(通用接口)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="args">字段, 符合(条件), 数据</param>
        /// <param name="cons">条件</param>
        /// <param name="value">更新值</param>
        /// <returns></returns>
        public void CommonUpdate<T>(IEnumerable<Tuple<string, string>> args, IEnumerable<Tuple<string, string>> cons, T value) where T : class
        {
            CommonUpdate<T>(args, cons, new List<T> { value });
        }
        public void CommonUpdate<T>(IEnumerable<Tuple<string, string>> args, IEnumerable<Tuple<string, string>> cons, IEnumerable<T> values) where T : class
        {
            var field = args.Select(x => $"`{x.Item1}` {x.Item2} @{x.Item1}").Join(", ");
            var con = cons.Select(x => $"`{x.Item1}` {x.Item2} @{x.Item1}").Join(" AND ");
            CommonUpdate(field, con, values);
        }
        public void CommonUpdate<T>(IEnumerable<Tuple<string, string>> args, IEnumerable<Tuple<string, string>> cons, List<T> values) where T : class
        {
            var field = args.Select(x => $"`{x.Item1}` {x.Item2} @{x.Item1}").Join(", ");
            var con = cons.Select(x => $"`{x.Item1}` {x.Item2} @{x.Item1}").Join(" AND ");
            CommonUpdate(field, con, values);
        }
        public void CommonUpdate<T>(IEnumerable<string> args, IEnumerable<string> cons, T value) where T : class
        {
            CommonUpdate<T>(args, cons, new List<T> { value });
        }
        public void CommonUpdate<T>(IEnumerable<string> args, IEnumerable<string> cons, IEnumerable<T> values) where T : class
        {
            var field = args.Select(x => $"`{x}` = @{x}").Join(", ");
            var con = cons.Select(x => $"`{x}` = @{x}").Join(" AND ");
            CommonUpdate(field, con, values);
        }
        public void CommonUpdate<T>(IEnumerable<string> args, IEnumerable<string> cons, List<T> values) where T : class
        {
            var field = args.Select(x => $"`{x}` = @{x}").Join(", ");
            var con = cons.Select(x => $"`{x}` = @{x}").Join(" AND ");
            CommonUpdate(field, con, values);
        }
        private void CommonUpdate<T>(string field, string con, IEnumerable<T> values) where T : class
        {
            DB.Execute($"UPDATE `{Table}` SET {field} WHERE {con};", values);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 单个删除
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            DB.Execute($"UPDATE `{Table}` SET `MarkedDateTime`= NOW(), `MarkedDelete`= true WHERE `Id`= @id;", new { id });
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        public void Delete(IEnumerable<int> ids)
        {
            DB.Execute($"UPDATE `{Table}` SET `MarkedDateTime`= NOW(), `MarkedDelete`= true WHERE `Id`IN @ids;", new { ids });
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="id"></param>
        public void DeleteFromParent(int id)
        {
            DB.Execute($"UPDATE `{Table}` SET `MarkedDateTime`= NOW(), `MarkedDelete`= true WHERE `{ParentField}` = @id;", new { id });
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        public void DeleteFromParent(IEnumerable<int> ids)
        {
            DB.Execute($"UPDATE `{Table}` SET `MarkedDateTime`= NOW(), `MarkedDelete`= true WHERE `{ParentField}` IN @ids;", new { ids });
        }
        #endregion

    }
}

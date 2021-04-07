namespace ModelBase.Models.Server
{
    public class ServerDataBase
    {
        public int Id { get; set; }
        /// <summary>
        /// 1:API;2:Data;
        /// </summary>
        public DataBaseType Type { get; set; }
        public string Name { get; set; }
        public string DataBase { get; set; }
        public bool Read { get; set; }
        public bool Write { get; set; }
    }

    public enum DataBaseType
    {
        None = 0,
        Api,
        Data,
    }
}

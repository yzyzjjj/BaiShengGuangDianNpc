using System;

namespace NpcProxyLinkClient.Models
{
    public class UsuallyDictionary
    {
        public int Id { get; set; }
        public string CreateUserId { get; set; }
        public DateTime MarkedDateTime { get; set; }
        public bool MarkedDelete { get; set; }
        public int ModifyId { get; set; }
        public int ScriptId { get; set; }
        public int VariableNameId { get; set; }
        public int DictionaryId { get; set; }

    }
}

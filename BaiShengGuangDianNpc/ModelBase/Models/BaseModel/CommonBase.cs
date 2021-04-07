using System;
using ModelBase.Base.Logic;
using Newtonsoft.Json;

namespace ModelBase.Models.BaseModel
{
    public class CommonBase
    {
        [IgnoreChange]
        public int Id { get; set; }
        [IgnoreChange]
        [JsonIgnore]
        public string CreateUserId { get; set; } = "";
        [IgnoreChange]
        public DateTime MarkedDateTime { get; set; }
        [IgnoreChange]
        [JsonIgnore]
        public bool MarkedDelete { get; set; } = false;
        [IgnoreChange]
        [JsonIgnore]
        public int ModifyId { get; set; } = 0;
    }
}

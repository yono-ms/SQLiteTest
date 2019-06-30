using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SQLiteTest
{
    public class ZipcodeItem
    {
        [PrimaryKey, AutoIncrement]
        [JsonIgnore]
        public long Id { get; set; }
        [Indexed]
        [JsonIgnore]
        public long UpdateValue { get; set; }
        [JsonIgnore]
        public string UpdateText { get; set; }
        public string Zipcode { get; set; }
        public string Prefcode { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Kana1 { get; set; }
        public string Kana2 { get; set; }
        public string Kana3 { get; set; }
    }
}

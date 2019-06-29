using System;
using System.Collections.Generic;
using System.Text;

namespace SQLiteTest
{
    public class ZipcodeSearchResponse
    {
        public long Status { get; set; }
        public string Message { get; set; }
        public List<ZipcodeItem> Results { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SQLiteTest
{
    public static class ZipcodeProvider
    {
        public static (string, List<ZipcodeItem>) Search(string zipcode)
        {
            return ("error message", null);
        }
    }
}

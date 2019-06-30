using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace SQLiteTest
{
    public static class AppLog
    {
        public static void Debug(string message, [CallerFilePath] string path = "None", [CallerMemberName] string name = "None", [CallerLineNumber] int line = -1)
        {
            System.Diagnostics.Debug.WriteLine($"[DEBUG] {Path.GetFileName(path)}({line}) {name} {message}");
        }
        public static void Error(Exception ex, [CallerFilePath] string path = "None", [CallerMemberName] string name = "None", [CallerLineNumber] int line = -1)
        {
            System.Diagnostics.Debug.WriteLine($"[ERROR] {Path.GetFileName(path)}({line}) {name} {ex.ToString()}");
        }
    }
}

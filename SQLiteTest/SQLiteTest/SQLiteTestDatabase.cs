using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SQLiteTest
{
    public class SQLiteTestDatabase
    {
        readonly SQLiteAsyncConnection database;

        public SQLiteTestDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<ZipcodeItem>().Wait();
        }
    }
}

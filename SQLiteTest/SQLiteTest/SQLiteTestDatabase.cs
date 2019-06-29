using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteTest
{
    /// <summary>
    /// データベース管理
    /// </summary>
    public class SQLiteTestDatabase
    {
        readonly SQLiteAsyncConnection database;

        public SQLiteTestDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<ZipcodeItem>().Wait();
        }

        public async Task<bool> IsExixtZipcodeAsync(string zipcode)
        {
            var items = await database.Table<ZipcodeItem>().ToListAsync();
            return items.Any(i => string.Equals(zipcode, i.Zipcode));
        }
        public async Task<List<ZipcodeItem>> GetZipcodeItemsAsync(string zipcode)
        {
            var items = await database.Table<ZipcodeItem>().ToListAsync();
            return items.Where(i => string.Equals(zipcode, i.Zipcode)).ToList();
        }

        public async Task AddZipcodeItemsAsync(List<ZipcodeItem> zipcodeItems)
        {
            foreach (var item in zipcodeItems)
            {
                await database.InsertOrReplaceAsync(item);
            }
        }
    }
}

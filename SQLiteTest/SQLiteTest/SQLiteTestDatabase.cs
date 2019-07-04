using SQLite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
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
            try
            {
                database.CreateTableAsync<ZipcodeItem>().Wait();
            }
            catch (Exception ex)
            {
                AppLog.Error(ex);
                database.DropTableAsync<ZipcodeItem>().Wait();
                database.CreateTableAsync<ZipcodeItem>().Wait();
            }
            database.CreateIndexAsync(nameof(ZipcodeItem), nameof(ZipcodeItem.UpdateValue)).Wait();
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
                var now = DateTime.Now;

                CultureInfo originalCulture = Thread.CurrentThread.CurrentCulture;
                Thread.CurrentThread.CurrentCulture = new CultureInfo("ja-JP");
                item.UpdateText = now.ToShortDateString();
                item.UpdateValue = now.Ticks;
                Thread.CurrentThread.CurrentCulture = originalCulture;

                await database.InsertAsync(item);
            }
        }
        public async Task<List<ZipcodeItem>> GetZipcodeItemsAsync()
        {
            return await database.Table<ZipcodeItem>().OrderBy(e => e.Prefcode).ThenBy(e => e.Zipcode).ToListAsync();
        }
        public async Task<List<ZipcodeItem>> GetZipcodeItemsRecentlyAsync()
        {
            var limit = DateTime.Now.AddHours(-1).Ticks;
            return await database.Table<ZipcodeItem>().Where(e => e.UpdateValue > limit).ToListAsync();
        }
        public async Task<List<ZipcodeItem>> GetZipcodeItemsPrefcodeAsync(string prefcode)
        {
            return await database.Table<ZipcodeItem>().Where(e => e.Prefcode == prefcode).ToListAsync();
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteTest
{
    public static class ZipcodeProvider
    {
        public static async Task<(string message, List<ZipcodeItem> zipcodeItems)> SearchAsync(string zipcode)
        {
            var isExist = await App.Database.IsExixtZipcodeAsync(zipcode);
            if (isExist)
            {
                AppLog.Debug($"データベースにキャッシュ済み");
                var zipcodeItems = await App.Database.GetZipcodeItemsAsync(zipcode);
                return (null, zipcodeItems);
            }

            try
            {
                var parameters = new Dictionary<string, string>()
                {
                    { "zipcode", zipcode },
                };
                var paramString = await new FormUrlEncodedContent(parameters).ReadAsStringAsync();
                var apiString = $"http://zipcloud.ibsnet.co.jp/api/search";
                var urlString = $"{apiString}?{paramString}";

                using (var httpClient = new HttpClient())
                {
                    AppLog.Debug($"--SEND-- {urlString}");
                    var json = await httpClient.GetStringAsync(urlString);
                    AppLog.Debug($"--RECV-- {json}");
                    var response = JsonConvert.DeserializeObject<ZipcodeSearchResponse>(json);
                    AppLog.Debug($"Status={response.Status}");
                    if (response.Status == 200)
                    {
                        AppLog.Debug($"正常終了コード");
                        if (response.Results == null)
                        {
                            AppLog.Debug($"検索結果が無い");
                            return ("登録されていません。", null);
                        }
                        else
                        {
                            AppLog.Debug($"検索結果={response.Results.Count}件");
                            await App.Database.AddZipcodeItemsAsync(response.Results);
                            return (null, response.Results);
                        }
                    }
                    else
                    {
                        AppLog.Debug($"異常終了コード");
                        return (response.Message, null);
                    }
                }
            }
            catch (Exception ex)
            {
                AppLog.Error(ex);
                return (ex.Message, null);
            }
        }
    }
}

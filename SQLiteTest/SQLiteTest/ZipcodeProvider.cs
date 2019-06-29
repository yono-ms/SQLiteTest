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
                    var json = await httpClient.GetStringAsync(urlString);
                    var response = JsonConvert.DeserializeObject<ZipcodeSearchResponse>(json);
                    if (response.Status == 200)
                    {
                        if (response.Results == null)
                        {
                            return ("登録されていません。", null);
                        }
                        else
                        {
                            return (null, response.Results);
                        }
                    }
                    else
                    {
                        return (response.Message, null);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                return (ex.Message, null);
            }
        }
    }
}

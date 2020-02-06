using Newtonsoft.Json;
using SkydevCSTool.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1;

namespace SkydevCSTool.Models
{
    public class InternalRequestReview
    {
        public int id { get; set; }
        public string url { get; set; }
        public int agent_id { get; set; }
        public string agent_notes { get; set; }
        public string status { get; set; }
        public string reviewer_notes { get; set; }
        public string updated_at { get; set; }


        public InternalRequestReview Save()
        {
            Globals.SaveToLogFile(string.Concat("Save IRS: ", JsonConvert.SerializeObject(this)), (int)LogType.Action);
            using (var client = new HttpClient())
            {
                var uri = string.Concat(Url.API_URL, "/irs/");
                client.DefaultRequestHeaders.Add("Authorization", Globals.apiKey);
                client.Timeout = TimeSpan.FromSeconds(5);
                var content = new StringContent(JsonConvert.SerializeObject(this), Encoding.UTF8, "application/json");
                var response = client.PostAsync(uri, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    using (HttpContent data = response.Content)
                    {
                        var jsonString = data.ReadAsStringAsync();
                        jsonString.Wait();
                        return JsonConvert.DeserializeObject<InternalRequestReview>(jsonString.Result);
                    }
                }
                else
                {
                    Globals.SaveToLogFile(JsonConvert.SerializeObject(this), (int)LogType.Error);
                    throw new Exception("Api save rr request error, Please contact dev team");
                }
            }
        }

        public static InternalRequestReview Get(int rr_id)
        {
            using (var client = new HttpClient())
            {
                var appversion = Globals.CurrentVersion().ToString().Replace(".", "");
                var uri = string.Concat(Url.API_URL, "/irs/", rr_id);
                client.DefaultRequestHeaders.Add("Authorization", Globals.apiKey);
                using (HttpResponseMessage response = client.SendAsync(new HttpRequestMessage(HttpMethod.Get, uri)).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        using (HttpContent content = response.Content)
                        {
                            var jsonString = content.ReadAsStringAsync();
                            jsonString.Wait();
                            return JsonConvert.DeserializeObject<InternalRequestReview>(jsonString.Result);
                        }
                    }
                }
            }

            return null;
        }
    }
}

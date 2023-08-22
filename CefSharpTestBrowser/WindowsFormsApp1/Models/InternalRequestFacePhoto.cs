using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1;
using CSTool.Class;

namespace CSTool.Models
{
    public class irfp_result
    {
        public List<InternalRequestFacePhoto> irfp { get; set; }
    }
    public class InternalRequestFacePhoto
    {
        public int id { get; set; }
        public string url { get; set; }
        public int agent_id { get; set; }
        public string status { get; set; }
        public string updated_at { get; set; }
        public int duration { get; set; }


        public InternalRequestFacePhoto Save()
        {
            Globals.SaveToLogFile(string.Concat("Save IRFP: ", JsonConvert.SerializeObject(this)), (int)LogType.Action);
            using (var client = new HttpClient())
            {
                var uri = string.Concat(Url.API_URL, "/irfp/");
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
                        return JsonConvert.DeserializeObject<InternalRequestFacePhoto>(jsonString.Result);
                    }
                }
                else
                {
                    Globals.SaveToLogFile(JsonConvert.SerializeObject(this), (int)LogType.Error);
                    throw new Exception("Api save irfp request error, Please contact dev team");
                }
            }
        }

        public static InternalRequestFacePhoto Get(int irfp_id)
        {
            using (var client = new HttpClient())
            {
                var appversion = Globals.CurrentVersion().ToString().Replace(".", "");
                var uri = string.Concat(Url.API_URL, "/irfp/", irfp_id, "/");
                client.DefaultRequestHeaders.Add("Authorization", Globals.apiKey);
                using (HttpResponseMessage response = client.SendAsync(new HttpRequestMessage(HttpMethod.Get, uri)).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        using (HttpContent content = response.Content)
                        {
                            var jsonString = content.ReadAsStringAsync();
                            jsonString.Wait();
                            return JsonConvert.DeserializeObject<InternalRequestFacePhoto>(jsonString.Result);
                        }
                    }
                }
            }

            return null;
        }

        public static irfp_result Get(List<int> agent_ids)
        {
            using (var client = new HttpClient())
            {
                var appversion = Globals.CurrentVersion().ToString().Replace(".", "");
                var uri = string.Concat(Url.API_URL, "/irfp/agent/", string.Join(",", agent_ids), "/");
                client.DefaultRequestHeaders.Add("Authorization", Globals.apiKey);
                using (HttpResponseMessage response = client.SendAsync(new HttpRequestMessage(HttpMethod.Get, uri)).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        using (HttpContent content = response.Content)
                        {
                            var jsonString = content.ReadAsStringAsync();
                            jsonString.Wait();
                            return JsonConvert.DeserializeObject<irfp_result>(jsonString.Result);
                        }
                    }
                }
            }

            return null;
        }
    }
}

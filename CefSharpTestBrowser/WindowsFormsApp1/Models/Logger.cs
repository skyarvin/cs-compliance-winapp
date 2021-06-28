using Newtonsoft.Json;
using CSTool.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1.Models
{
    public class Logger
    {
        public int id { get; set; }
        public string agent_id { get; set; }
        public string url { get; set; }
        public string action { get; set; }
        public string remarks { get; set; }
        public double duration { get; set; }
        public int followers { get; set; }
        public bool sc { get; set; }
        public bool rr { get; set; }
        public string workshift { get; set; }
        public string review_date { get; set; }
        public string last_chatlog { get; set; }
        public string last_photo { get; set; }
        public string hash { get; set; }
        public string actual_start_time { get; set; }
        public string actual_end_time { get; set; }
        public List<Profile> members { get; set; }
        public int? irs_id { get; set; }
        public int? idc_id { get; set; }
        public bool is_trainee { get; set; }

        public Logger Save()
        {
            Globals.SaveToLogFile(string.Concat("Save: ", JsonConvert.SerializeObject(this)), (int)LogType.Action);
            using (var client = new HttpClient())
            {
                var uri = string.Concat(Url.API_URL, "/logs/");
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
                        return JsonConvert.DeserializeObject<Logger>(jsonString.Result);
                    }
                }
                else
                {
                    Globals.SaveToLogFile(JsonConvert.SerializeObject(this), (int)LogType.Error);
                    Resync.SavetoDB(JsonConvert.SerializeObject(this), "Save");
                    throw new Exception("Api save request error, Please contact dev team");
                }
            }
        }

        public void Update()
        {
            Globals.SaveToLogFile(string.Concat("Update: ", JsonConvert.SerializeObject(this)), (int)LogType.Action);
            using (var client = new HttpClient())
            {
                var uri = string.Concat(Url.API_URL, "/logs/", this.id);
                client.DefaultRequestHeaders.Add("Authorization", Globals.apiKey);
                var content = new StringContent(JsonConvert.SerializeObject(this), Encoding.UTF8, "application/json");
                var response = client.PutAsync(uri, content).Result;
                if (!response.IsSuccessStatusCode)
                {
                    Globals.SaveToLogFile(JsonConvert.SerializeObject(this), (int)LogType.Error);
                    Resync.SavetoDB(JsonConvert.SerializeObject(this), "Update");
                    throw new Exception("Api Update request error, Please contact dev team");
                }
            }
        }


        //Static Methods
        #region Static Methods
        public static UrlInformation GetUrlInformation(string url)
        {
            using (var client = new HttpClient())
            {
                var uri = string.Concat(Url.API_URL, "/logs/url_info");
                client.DefaultRequestHeaders.Add("Authorization", Globals.apiKey);
                var content = new StringContent("{\"compliance_url\":\"" + url + "\"}", Encoding.UTF8, "application/json");
                var response = client.PostAsync(uri, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        using (HttpContent data = response.Content)
                        {
                            var jsonString = data.ReadAsStringAsync();
                            jsonString.Wait();
                            return JsonConvert.DeserializeObject<UrlInformation>(jsonString.Result);
                        }
                    }

                    return new UrlInformation();
                    }
                else
                {
                    throw new Exception("Api save request error, Please contact dev team");
                }
            }
        }
        private static bool Process_Json_String(bool is_post, string json_string)
        {
            using (var client = new HttpClient())
            {
                var uri = string.Concat(Url.API_URL, "/logs/");
                client.DefaultRequestHeaders.Add("Authorization", Globals.apiKey);
                var content = new StringContent(json_string, Encoding.UTF8, "application/json");
                HttpResponseMessage response;
                if (is_post)
                    response = client.PostAsync(uri, content).Result;
                else
                    response = client.PutAsync(string.Concat(uri, JsonConvert.DeserializeObject<Logger>(json_string).id), content).Result;
                if (response.IsSuccessStatusCode)
                    return true;
            }
            return false;
        }

        public static bool Save_Json_String(string json_string)
        {
            return Process_Json_String(true, json_string);
        }

        public static bool Update_Json_String(string json_string)
        {
            return Process_Json_String(false, json_string);
        }
        #endregion
    }

    public class UrlInformation
    {
        public string last_chatlog { get; set; }
        public string last_photo { get; set; }
    }
}

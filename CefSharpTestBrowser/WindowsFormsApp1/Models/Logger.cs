using Newtonsoft.Json;
using SkydevCSTool.Class;
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
        public int duration { get; set; }
        public int followers { get; set; }
        public bool sc { get; set; }
        public bool rr { get; set; }

        public Logger Save()
        {
            Globals.SaveToLogFile(string.Concat("Save: ", JsonConvert.SerializeObject(this)), (int)LogType.Action);
            using (var client = new HttpClient())
            {
                var uri = string.Concat(Url.API_URL, "/logs/");
                client.DefaultRequestHeaders.Add("Authorization", Globals.apiKey);
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
                    throw new Exception("Api save request error");
                }
            }
        }

        public bool Update()
        {
            Globals.SaveToLogFile(string.Concat("Update: ", JsonConvert.SerializeObject(this)), (int)LogType.Action);
            using (var client = new HttpClient())
            {
                var uri = string.Concat(Url.API_URL, "/logs/", this.id);
                client.DefaultRequestHeaders.Add("Authorization", Globals.apiKey);
                var content = new StringContent(JsonConvert.SerializeObject(this), Encoding.UTF8, "application/json");
                var response = client.PutAsync(uri, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    Globals.SaveToLogFile(JsonConvert.SerializeObject(this), (int)LogType.Error);
                    Resync.SavetoDB(JsonConvert.SerializeObject(this), "Update");
                    throw new Exception("Api Update request error");
                }
            }
        }

    }
}

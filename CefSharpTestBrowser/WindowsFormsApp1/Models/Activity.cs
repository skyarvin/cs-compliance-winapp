using Newtonsoft.Json;
using SkydevCSTool.Class;
using System;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using WindowsFormsApp1;

namespace SkydevCSTool.Models
{
    public class Activity
    {
        public int? id { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public int agent_id { get; set; }
        public string work_date { get; set; }

        public void Save()
        {
            Globals.SaveToLogFile(string.Concat("Save Activity: ", JsonConvert.SerializeObject(this)), (int)LogType.Activity);
            using (var client = new HttpClient())
            {
                var uri = string.Concat(Url.API_URL, "/activity/"); ;
                client.DefaultRequestHeaders.Add("Authorization", Globals.apiKey);
                var content = new StringContent(JsonConvert.SerializeObject(this), Encoding.UTF8, "application/json");
                var response = client.PostAsync(uri, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    using (HttpContent data = response.Content)
                    {
                        var jsonString = data.ReadAsStringAsync();
                        jsonString.Wait();
                        this.id = JsonConvert.DeserializeObject<Activity>(jsonString.Result).id;
                    }
                }
                else
                {
                    Globals.SaveToLogFile(JsonConvert.SerializeObject(this), (int)LogType.Error);
                    throw new Exception("Api Activity save request error, Please contact dev team");
                }
            }

        }

        public void Update()
        {
            Globals.SaveToLogFile(string.Concat("Update Activity: ", JsonConvert.SerializeObject(this)), (int)LogType.Activity);
            using (var client = new HttpClient())
            {
                var uri = string.Concat(Url.API_URL, "/activity/", this.id); ;
                client.DefaultRequestHeaders.Add("Authorization", Globals.apiKey);
                var content = new StringContent(JsonConvert.SerializeObject(this), Encoding.UTF8, "application/json");
                var response = client.PutAsync(uri, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    using (HttpContent data = response.Content)
                    {
                        var jsonString = data.ReadAsStringAsync();
                        jsonString.Wait();
                        this.id = JsonConvert.DeserializeObject<Activity>(jsonString.Result).id;
                    }
                }
                else
                {
                    Globals.SaveToLogFile(JsonConvert.SerializeObject(this), (int)LogType.Error);
                    throw new Exception("Api Activity update request error, Please contact dev team");
                }
            }

        }
    }
}

using Newtonsoft.Json;
using CSTool.Class;
using System;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using WindowsFormsApp1;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Mime;
using WindowsFormsApp1.Models;

namespace CSTool.Models
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

        public static Activity Get(int agent_id)
        {
            using (var client = new HttpClient())
            {
                var appversion = Globals.CurrentVersion().ToString().Replace(".", "");
                var uri = string.Concat(Url.API_URL, "/activity/agent/?agent_id=", agent_id, "&version=", appversion);
                client.DefaultRequestHeaders.Add("Authorization", Globals.apiKey);
                using (HttpResponseMessage response = client.SendAsync(new HttpRequestMessage(HttpMethod.Get, uri)).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        using (HttpContent content = response.Content)
                        {
                            var jsonString = content.ReadAsStringAsync();
                            jsonString.Wait();
                            return JsonConvert.DeserializeObject<Activity>(jsonString.Result);
                        }
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Gone)
                    {
                        MessageBox.Show("Invalid app version. Please update your application.", "Error");
                        Application.Exit();
                    }
                }
            }

            return null;
        }

        public bool PostScreenshot(string filename)
        {
            return post("/agent/capture/sc/", filename); 
        }
        public bool PostCameraCapture(string filename)
        {
            return post("/agent/capture/wc/", filename); 
        }
        private bool post(string url, string fileAddress)
        {
            try
            {
                HttpClient client = new HttpClient();
                MultipartFormDataContent form = new MultipartFormDataContent();
                HttpContent content = new StringContent("fileToUpload");
                client.DefaultRequestHeaders.Add("Authorization", Globals.apiKey);
                form.Add(new StringContent(agent_id.ToString(), Encoding.UTF8, MediaTypeNames.Text.Plain), "agent");
                form.Add(content, "fileToUpload");
                var stream = new FileStream(fileAddress, FileMode.Open);
                content = new StreamContent(stream);
                var fileName = content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "snapshot",
                    FileName = Path.GetFileName(fileAddress),
                };
                form.Add(content);
                HttpResponseMessage response = null;
                var _url = new Uri(string.Concat(Url.API_URL, url));
                response = (client.PostAsync(_url, form)).Result;
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            catch
            {
                Globals.SaveToLogFile(string.Concat("Failed to upload: ", fileAddress), (int)LogType.Error);
                return false;
            }
            return false;
        }
    }
}

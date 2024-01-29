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
using CSTool.Handlers;
using CSTool.Handlers.Interfaces;
using CSTool.Handlers.ErrorsHandler;

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
            try
            {
                Globals.SaveToLogFile(string.Concat("Save Activity: ", JsonConvert.SerializeObject(this)), (int)LogType.Activity);
                using (IHttpHandler client = new HttpHandler())
                {
                    var uri = string.Concat(Url.API_URL, "/activity/");
                    var content = new StringContent(JsonConvert.SerializeObject(this), Encoding.UTF8, "application/json");
                    var response = client.CustomPostAsync(uri, content).Result;
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
            catch (AggregateException e) when (e.InnerException is UnauthorizeException)
            {
                Globals.SessionExpired();
                throw e;
            }
        }

        public void Update()
        {
            try
            {
                Globals.SaveToLogFile(string.Concat("Update Activity: ", JsonConvert.SerializeObject(this)), (int)LogType.Activity);
                using (IHttpHandler client = new HttpHandler())
                {
                    var uri = string.Concat(Url.API_URL, "/activity/", this.id);
                    var content = new StringContent(JsonConvert.SerializeObject(this), Encoding.UTF8, "application/json");
                    var response = client.CustomPutAsync(uri, content).Result;
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
            catch (AggregateException e) when (e.InnerException is UnauthorizeException)
            {
                Globals.SessionExpired();
                throw e;
            }
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
                IHttpHandler client = new HttpHandler();
                MultipartFormDataContent form = new MultipartFormDataContent();
                HttpContent content = new StringContent("fileToUpload");
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
                var _url = string.Concat(Url.API_URL, url);
                response = (client.CustomPostAsync(_url, form)).Result;
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            catch (AggregateException e) when (e.InnerException is UnauthorizeException)
            {
                Globals.SaveToLogFile(string.Concat("Failed to upload: ", fileAddress), (int)LogType.Error);
                Globals.SessionExpired();
                return false;
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

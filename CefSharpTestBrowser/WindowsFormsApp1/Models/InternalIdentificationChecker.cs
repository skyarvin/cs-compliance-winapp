using System;
using System.Text;
using Newtonsoft.Json;
using CSTool.Class;
using System.Net.Http;
using WindowsFormsApp1;
using System.IO;
using System.Net.Http.Headers;

namespace CSTool.Models
{
    public class InternalIdentificationChecker
    {
        public int id { get; set; }
        public string url { get; set; }
        public int agent_id { get; set; }
        public string agent_notes { get; set; }
        public string agent_uploaded_photo { get; set; }
        public string status { get; set; }
        public string reviewer_notes { get; set; }
        public string reviewer_uploaded_photo { get; set; }
        public string updated_at { get; set; }
        public int duration { get; set; }

        public InternalIdentificationChecker Save()
        {
            Globals.SaveToLogFile(string.Concat("Save IIDC: ", JsonConvert.SerializeObject(this)), (int)LogType.Action);
            using (var client = new HttpClient())
            {
                using (var form = new MultipartFormDataContent())
                {
                    client.DefaultRequestHeaders.Add("Authorization", Globals.apiKey);
                    client.Timeout = TimeSpan.FromSeconds(60);
                    form.Add(new StringContent(this.agent_id.ToString()), "agent");
                    form.Add(new StringContent(this.agent_notes), "agent_notes");
                    form.Add(new StringContent(this.url), "url");
                    form.Add(new StringContent(this.duration.ToString()), "duration");
                    if (!String.IsNullOrEmpty(this.agent_uploaded_photo))
                    {
                        HttpContent content = new StreamContent(new FileStream(this.agent_uploaded_photo, FileMode.Open));
                        content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                        {
                            Name = "agent_uploaded_photo",
                            FileName = Path.GetFileName(this.agent_uploaded_photo),
                        };
                        form.Add(content);
                    }
                    var uri = new Uri(string.Concat(Url.API_URL, "/iidc/"));
                    HttpResponseMessage response = client.PostAsync(uri, form).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        using (HttpContent data = response.Content)
                        {
                            var jsonString = data.ReadAsStringAsync();
                            jsonString.Wait();
                            return JsonConvert.DeserializeObject<InternalIdentificationChecker>(jsonString.Result);
                        }
                    }
                    else
                    {
                        Globals.SaveToLogFile(JsonConvert.SerializeObject(this), (int)LogType.Error);
                        throw new Exception("Api save iidc request error, Please contact dev team");
                    }
                }
            }
        }

        public static InternalIdentificationChecker Get(int iidc_id, int agent_id)
        {

            using (var client = new HttpClient())
            {
                var uri = string.Concat(Url.API_URL, "/iidc/", iidc_id, "/", agent_id, "/");
                client.DefaultRequestHeaders.Add("Authorization", Globals.apiKey);
                using (HttpResponseMessage response = client.SendAsync(new HttpRequestMessage(HttpMethod.Get, uri)).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        using (HttpContent content = response.Content)
                        {
                            var jsonString = content.ReadAsStringAsync();
                            jsonString.Wait();
                            return JsonConvert.DeserializeObject<InternalIdentificationChecker>(jsonString.Result);
                        }
                    }
                }
            }
            return null;
        }

        public static InternalIdentificationChecker GetAgentIIDC(int agent_id)
        {
            using (var client = new HttpClient())
            {
                var uri = string.Concat(Url.API_URL, "/iidc/agent/", agent_id, "/");
                client.DefaultRequestHeaders.Add("Authorization", Globals.apiKey);
                using (HttpResponseMessage response = client.SendAsync(new HttpRequestMessage(HttpMethod.Get, uri)).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        using (HttpContent content = response.Content)
                        {
                            var jsonString = content.ReadAsStringAsync();
                            jsonString.Wait();
                            return JsonConvert.DeserializeObject<InternalIdentificationChecker>(jsonString.Result);
                        }
                    }
                }
            }
            return null;
        }
    }
}

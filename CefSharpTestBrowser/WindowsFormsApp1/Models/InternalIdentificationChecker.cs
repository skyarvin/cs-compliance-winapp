using System;
using System.Text;
using Newtonsoft.Json;
using CSTool.Class;
using System.Net.Http;
using WindowsFormsApp1;
using CSTool.Handlers.Interfaces;
using CSTool.Handlers;

namespace CSTool.Models
{
    public class InternalIdentificationChecker
    {
        public int id { get; set; }
        public string url { get; set; }
        public int agent_id { get; set; }
        public string agent_notes { get; set; }
        public string agent_uploaded_photo { get; set; }
        public string agent_uploaded_photo_base64 { get; set; }
        public string status { get; set; }
        public string reviewer_notes { get; set; }
        public string reviewer_uploaded_photo { get; set; }
        public string updated_at { get; set; }
        public int duration { get; set; }

        public InternalIdentificationChecker Save()
        {
            Globals.SaveToLogFile(string.Concat("Save IIDC: ", JsonConvert.SerializeObject(this)), (int)LogType.Action);
            using (IHttpHandler client = new HttpHandler())
            {
                var uri = string.Concat(Url.API_URL, "/iidc/");
                client.Timeout = TimeSpan.FromSeconds(60);
                var content = new StringContent(JsonConvert.SerializeObject(this), Encoding.UTF8, "application/json");
                var response = client.CustomPostAsync(uri, content).Result;
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

        public static InternalIdentificationChecker Get(int iidc_id, int agent_id)
        {

            using (IHttpHandler client = new HttpHandler())
            {
                var uri = string.Concat(Url.API_URL, "/iidc/", iidc_id, "/", agent_id, "/");
                using (HttpResponseMessage response = client.CustomGetAsync(uri).Result)
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
            using (IHttpHandler client = new HttpHandler())
            {
                var uri = string.Concat(Url.API_URL, "/iidc/agent/", agent_id, "/");
                using (HttpResponseMessage response = client.CustomGetAsync(uri).Result)
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

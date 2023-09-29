using Newtonsoft.Json;
using CSTool.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1;
using CSTool.Handlers;

namespace CSTool.Models
{
    public class IdChecker
    {
        public int id { get; set; }
        public int agent_id { get; set; }
        public string url { get; set; }
        public string status { get; set; }
        public string reviewer_notes { get; set; }
        public IdChecker Save()
        {
            Globals.SaveToLogFile(string.Concat("Send Id checker: ", JsonConvert.SerializeObject(this)), (int)LogType.Action);
            using (var client = new HttpHandler())
            {
                var uri = string.Concat(Url.API_URL, "/idc/?agent_id=", this.agent_id, "&url=", this.url);
                client.Timeout = TimeSpan.FromSeconds(5);
                var content = new StringContent(JsonConvert.SerializeObject(this), Encoding.UTF8, "application/json");
                var response = client.CPostAsync(uri, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    using (HttpContent data = response.Content)
                    {
                        var jsonString = data.ReadAsStringAsync();
                        jsonString.Wait();
                        return JsonConvert.DeserializeObject<IdChecker>(jsonString.Result);
                    }
                }
                else
                {
                    Globals.SaveToLogFile(JsonConvert.SerializeObject(this), (int)LogType.Error);
                    throw new Exception("Api send idchecker request error, Please contact dev team");
                }
            }
        }
        public static IdChecker Get(int id)
        {
            using (var client = new HttpHandler())
            {
                var appversion = Globals.CurrentVersion().ToString().Replace(".", "");
                var uri = string.Concat(Url.API_URL, "/idc/", id, "/");
                try
                {
                    using (HttpResponseMessage response = client.CGetAsync(uri).Result)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            using (HttpContent content = response.Content)
                            {
                                var jsonString = content.ReadAsStringAsync();
                                jsonString.Wait();
                                return JsonConvert.DeserializeObject<IdChecker>(jsonString.Result);
                            }
                        }
                        else
                        {
                            Globals.SaveToLogFile(string.Concat("idchecker id: ", id), (int)LogType.Error);
                        }
                    }
                }
                catch { Globals.SaveToLogFile(string.Concat("idchecker id: ", id), (int)LogType.Error); }
            }

            return null;
        }
    }
}

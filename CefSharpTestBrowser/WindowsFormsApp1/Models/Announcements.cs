using Newtonsoft.Json;
using CSTool.Class;
using System;
using System.Net.Http;
using System.Text;
using WindowsFormsApp1;
using System.Collections.Generic;
using CSTool.Handlers;

namespace CSTool.Models
{
    class Announcements
    {
        public List<Announcement> announcements { get; set; }

        public static Announcements FetchAnnouncements()
        {
            using (var client = new HttpHandler())
            {
                var agent_id = Globals.ComplianceAgent.id;
                var uri = $"{Url.API_URL}/agent/{agent_id}/announcements/";
                var response = client.CGetAsync(uri).Result;
                if (response.IsSuccessStatusCode)
                {
                    using (HttpContent content = response.Content)
                    {
                        var rawJsonString = content.ReadAsStringAsync();
                        rawJsonString.Wait();
                        return JsonConvert.DeserializeObject<Announcements>(rawJsonString.Result);
                    }
                }
            }
            return null;
        }
    }

    public class Announcement
    {

        public int id { get; set; }

        public string title { get; set; }

        public string message { get; set; }

        public string link_to_post { get; set; }

        public bool publish_status { get; set; }

        public string created_at { get; set; }

        public string updated_at { get; set; }

        public bool read_status { get; set; }

        public int? agent_id { get; set; }

        public string time_since { get; set; }

        public void AcknowledgeAnnouncement()
        {
            using (var client = new HttpHandler())
            {
                this.agent_id = Globals.ComplianceAgent.id;
                var uri = $"{Url.API_URL}/announcement/{this.id}/acknowledge/";
                var content = new StringContent(JsonConvert.SerializeObject(this), Encoding.UTF8, "application/json");
                var response = client.CPostAsync(uri, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    using (HttpContent data = response.Content)
                    {
                        var jsonString = data.ReadAsStringAsync();
                        jsonString.Wait();
                        this.read_status = JsonConvert.DeserializeObject<Announcement>(jsonString.Result).read_status;
                    }
                }
                else
                {
                    Globals.SaveToLogFile(JsonConvert.SerializeObject(this), (int)LogType.Error);
                    throw new Exception("Api acknowledge of announcement request error, Please contact dev team");
                }
            }
        }

    }
}

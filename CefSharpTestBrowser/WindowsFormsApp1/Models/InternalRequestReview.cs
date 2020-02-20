using Newtonsoft.Json;
using SkydevCSTool.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1;

namespace SkydevCSTool.Models
{
    public class InternalRequestReview
    {
        public int id { get; set; }
        public string url { get; set; }
        public int agent_id { get; set; }
        public string agent_notes { get; set; }
        public string status { get; set; }
        public string reviewer_notes { get; set; }
        public string updated_at { get; set; }
        public int duration { get; set; }
        public string violation { get; set; }
        public static Dictionary<String, String> violations = new Dictionary<String, String>
        {
                {"", "" },
                {"BST", "Beastiality" },
                {"GTS","Goatse" },
                {"NST","Incest" },
                {"NON","No Nudity" },
                {"PPB","Pee, Poo, Blood" },
                {"UOC","With Nudity -- Underage on cam" },
                {"COC","Child On Cam" },
                {"FST","Fisting" },
                {"AHB","Abusive or Harmful Behavior" },
                {"AOP","Asking for offline payments" },
                {"BPP","Broadcasting in Public Places" },
                {"BMV","Broadcasting from a moving vehicle" },
                {"BSU","Do not Broadcast in Service Uniforms" },
                {"IOT","Inserting things other than proper sex toys" },
                {"EXD", "Intoxicated -- excessive drinking even if over 21" },
                {"EID","Intoxicated -- excessive or illegal drugs on cam" },
                {"OLT","Overly large toys" },
                {"SPV","Showing photos / videos of others is not allowed" },
                {"STV","Showing television / movies / videogames on cam is not allowed" },
                {"SYM","Skype, Yahoo Messenger" },
                {"SOC","Sleeping on Camera" },
                {"BAC","Broadcasting away from cam" },
                {"IDM","ID Missing" },
                {"SOR","Spammer/Recording" },
                {"RR","Request Review" },

        };
        public string violation_long_name
        {
            get
            { 
                if(!String.IsNullOrEmpty(violation) && violations.ContainsKey(violation))
                    return violations[violation];
                return "";
            }
        }


        public InternalRequestReview Save()
        {
            Globals.SaveToLogFile(string.Concat("Save IRS: ", JsonConvert.SerializeObject(this)), (int)LogType.Action);
            using (var client = new HttpClient())
            {
                var uri = string.Concat(Url.API_URL, "/irs/");
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
                        return JsonConvert.DeserializeObject<InternalRequestReview>(jsonString.Result);
                    }
                }
                else
                {
                    Globals.SaveToLogFile(JsonConvert.SerializeObject(this), (int)LogType.Error);
                    throw new Exception("Api save rr request error, Please contact dev team");
                }
            }
        }

        public static InternalRequestReview Get(int rr_id)
        {
            using (var client = new HttpClient())
            {
                var appversion = Globals.CurrentVersion().ToString().Replace(".", "");
                var uri = string.Concat(Url.API_URL, "/irs/", rr_id);
                client.DefaultRequestHeaders.Add("Authorization", Globals.apiKey);
                using (HttpResponseMessage response = client.SendAsync(new HttpRequestMessage(HttpMethod.Get, uri)).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        using (HttpContent content = response.Content)
                        {
                            var jsonString = content.ReadAsStringAsync();
                            jsonString.Wait();
                            return JsonConvert.DeserializeObject<InternalRequestReview>(jsonString.Result);
                        }
                    }
                }
            }

            return null;
        }
    }
}

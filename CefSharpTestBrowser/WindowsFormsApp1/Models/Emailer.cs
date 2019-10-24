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
    class Emailer
    {
        public string subject { get; set; }
        public string message { get; set; }


        public void Send()
        {
            using (var httpClient = new HttpClient())
            {
                string url = String.Concat(Url.API_URL, "/emailer/");
                httpClient.DefaultRequestHeaders.Add("Authorization", Globals.apiKey);
                var content = new StringContent(JsonConvert.SerializeObject(this), Encoding.UTF8, "application/json");
                try
                {
                    var response = httpClient.PostAsync(url, content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        using (HttpContent data = response.Content)
                        {
                            var jsonString = data.ReadAsStringAsync();
                            jsonString.Wait();
                        }
                    }
                    else
                    {
                        Globals.SaveToLogFile("EMAILER FAILED: " + JsonConvert.SerializeObject(this), (int)LogType.Error);
                    }
                } catch (AggregateException)
                {
                    Globals.SaveToLogFile("NETWORK ERROR: " + JsonConvert.SerializeObject(this), (int)LogType.Error);
                }
            }
        }
        
    }
}

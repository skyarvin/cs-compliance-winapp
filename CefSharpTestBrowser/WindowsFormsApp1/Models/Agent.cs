using Newtonsoft.Json;
using SkydevCSTool.Class;
using System;
using System.Net.Http;

namespace WindowsFormsApp1.Models
{
    public class Agent
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string photo { get; set; }

        public static Agent Get(string email)
        {
            using (var client = new HttpClient())
            {
                var uri = string.Concat(Url.API_URL, "/agents/", email);
                client.DefaultRequestHeaders.Add("Authorization", Globals.apiKey);
                using (HttpResponseMessage response = client.SendAsync(new HttpRequestMessage(HttpMethod.Get, uri)).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        using (HttpContent content = response.Content)
                        {
                            var jsonString = content.ReadAsStringAsync();
                            jsonString.Wait();
                            return JsonConvert.DeserializeObject<Agent>(jsonString.Result);
                        }
                    }
                    else
                    {
                        Globals.SaveToLogFile(email, (int)LogType.Error);
                        throw new Exception("Api get request error");
                    }
                }
            }
        }
    }
}

using CSTool.Handlers;
using CSTool.Class;
using CSTool.Handlers.Interfaces;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using WindowsFormsApp1;

namespace CSTool.Models
{
    internal class ServerTime
    {
        public string server_datetime { get; set; }

        public DateTime FetchServerTime()
        {
            using (IHttpHandler client = new HttpHandler())
            {
                try
                {
                    var uri = string.Concat(Url.API_URL, "/get_server_time");
                    var response = client.CustomGetAsync(uri).Result;
                    HttpContent data = response.Content;
                    var jsonString = data.ReadAsStringAsync();
                    jsonString.Wait();
                    ServerTime time = JsonConvert.DeserializeObject<ServerTime>(jsonString.Result);
                    return DateTimeOffset.Parse(time.server_datetime).DateTime;
                }
                catch(Exception e)
                {
                    Globals.SaveToLogFile(String.Concat("Failed to Fetch Time: ", e), (int)LogType.DateTime_Handler);
                    return DateTime.Now;
                }
            }
        }

        public TimeSpan GetTimeOffset()
        {
            DateTime result = FetchServerTime();
            return result - DateTime.Now;
        }

        public static DateTime Now() 
        {
            return DateTime.Now.Add(Globals.timeOffset);
        }

        public static DateTime UtcNow()
        {
            return DateTime.UtcNow.Add(Globals.timeOffset);
        }
    }
}

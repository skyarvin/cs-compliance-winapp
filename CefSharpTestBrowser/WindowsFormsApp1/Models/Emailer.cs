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
using CSTool.Handlers.Interfaces;
using CSTool.Handlers.ErrorsHandler;

namespace CSTool.Models
{
    class Emailer
    {
        public string subject { get; set; }
        public string message { get; set; }


        public void Send()
        {
            try
            {
                using (IHttpHandler httpClient = new HttpHandler())
                {
                    string url = String.Concat(Url.API_URL, "/emailer/");
                    var content = new StringContent(JsonConvert.SerializeObject(this), Encoding.UTF8, "application/json");
                    try
                    {
                        var response = httpClient.CustomPostAsync(url, content).Result;
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
                    }
                    catch (AggregateException)
                    {
                        Globals.SaveToLogFile("NETWORK ERROR: " + JsonConvert.SerializeObject(this), (int)LogType.Error);
                    }
                }
            }
            catch (AggregateException e) when (e.InnerException is UnauthorizeException)
            {
                Globals.SessionExpired();
                throw e;
            }
        }
        
    }
}

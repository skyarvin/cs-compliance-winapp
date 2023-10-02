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

namespace CSTool.Models
{
    public class Seed
    {
        public string  url { get; set; }
        public int log_id { get; set; }


        public void Save()
        {
            Globals.SaveToLogFile(string.Concat("Save Missed Seed: ", JsonConvert.SerializeObject(this)), (int)LogType.Activity);
            using (IHttpHandler client = new HttpHandler())
            {
                var uri = string.Concat(Url.API_URL, "/missed-seeds/"); ;
                var content = new StringContent(JsonConvert.SerializeObject(this), Encoding.UTF8, "application/json");
                var response = client.CustomPostAsync(uri, content).Result;
                if (!response.IsSuccessStatusCode)
                {               
                    Globals.SaveToLogFile(JsonConvert.SerializeObject(this), (int)LogType.Error);
                    throw new Exception("Api Missed Seed save request error, Please contact dev team");
                }
            }
        }

    }

}

using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using WindowsFormsApp1;

namespace SkydevCSTool.Models
{
    public class Activity
    {
        public string start_time { get; set; }
        public string end_time { get; set; }
        public int agent_id { get; set; }

        public void Save()
        {
            Globals.SaveToLogFile(string.Concat("Save Activity: ", JsonConvert.SerializeObject(this)), (int)LogType.Activity);
            try
            {
                using (var client = new HttpClient())
                {
                    var uri = string.Concat(Globals.baseUrl, "/activity/"); ;
                    client.DefaultRequestHeaders.Add("Authorization", Globals.apiKey);
                    var content = new StringContent(JsonConvert.SerializeObject(this), Encoding.UTF8, "application/json");
                    var response = client.PostAsync(uri, content).Result;
                    if (!response.IsSuccessStatusCode)
                    {
                        Globals.SaveToLogFile(JsonConvert.SerializeObject(this), (int)LogType.Error);
                        MessageBox.Show(String.Concat("Something went wrong.", System.Environment.NewLine, "Please contact Admin."), "Error");
                    }
                }
            }
            catch (Exception e)
            {
                Globals.SaveToLogFile(e.Message, (int)LogType.Error);
            }

        }

    }
}

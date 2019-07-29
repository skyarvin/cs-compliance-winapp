using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1.Models
{
    public class Logger
    {
        public int id { get; set; }
        public string agent_id { get; set; }
        public string url { get; set; }
        public string action { get; set; }
        public string remarks { get; set; }
        public int duration { get; set; }

        public int followers { get; set; }

        public Logger Save()
        {
            Globals.SaveToLogFile(string.Concat("Save: ", JsonConvert.SerializeObject(this)), (int)LogType.Action);
            try
            {
                using (var client = new HttpClient())
                {
                    var uri = string.Concat(Globals.baseUrl, "/logs/");
                    client.DefaultRequestHeaders.Add("Authorization", Globals.apiKey);
                    var content = new StringContent(JsonConvert.SerializeObject(this), Encoding.UTF8, "application/json");
                    var response = client.PostAsync(uri, content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        using (HttpContent data = response.Content)
                        {
                            var jsonString = data.ReadAsStringAsync();
                            jsonString.Wait();
                            return JsonConvert.DeserializeObject<Logger>(jsonString.Result);
                        }
                    }
                    else
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

            return null;
        }

        public bool Update()
        {
            Globals.SaveToLogFile(string.Concat("Update: ", JsonConvert.SerializeObject(this)), (int)LogType.Action);
            try
            {
                using (var client = new HttpClient())
                {
                    var uri = string.Concat(Globals.baseUrl, "/logs/", this.id);
                    client.DefaultRequestHeaders.Add("Authorization", Globals.apiKey);
                    var content = new StringContent(JsonConvert.SerializeObject(this), Encoding.UTF8, "application/json");
                    var response = client.PutAsync(uri, content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
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

            return false;
        }

    }
}

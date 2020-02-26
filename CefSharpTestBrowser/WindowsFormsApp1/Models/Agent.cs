using Newtonsoft.Json;
using SkydevCSTool.Class;
using System;
using System.Net.Http;
using System.Windows.Forms;

namespace WindowsFormsApp1.Models
{
    public class Agent
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string photo { get; set; }
        //public string review_date { get; set; }
        //public string last_workshift { get; set; }
        public string role { get; set; }
        public string profile
        {
            get
            {
                var email_ = email.Split('@');
                return email_[0];
            }
        }
        public bool is_trainee
        {
            get
            {
                if (!String.IsNullOrEmpty(role) && role.ToUpper().Contains("TRAINEE"))
                    return true;
                return false;
            }
        }
        public static Agent Get(string email)
        {
            using (var client = new HttpClient())
            {
                var appversion = Globals.CurrentVersion().ToString().Replace(".", "");
                var uri = string.Concat(Url.API_URL, "/agent/?email=", email,"&version=", appversion);
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
                    }else if(response.StatusCode == System.Net.HttpStatusCode.Gone) {
                        MessageBox.Show("Invalid app version. Please update your application.", "Error");
                        Application.Exit();
                    }
                }
            }

            return null;
        }
    }
}

using Newtonsoft.Json;
using CSTool.Class;
using System;
using System.Net.Http;
using System.Windows.Forms;
using CSTool.Properties;

namespace WindowsFormsApp1.Models
{
    public class Agent
    {
        public int id { get; set; }
        public string name { get; set; }
        public int? tier_level { get; set; }
        public string email { get; set; }
        public string photo { get; set; }
        //public string review_date { get; set; }
        //public string last_workshift { get; set; }
        public string role { get; set; }
        public bool id_checking { get; set; }
        public bool webcam_capture { get; set; }
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
                if (Settings.Default.user_type.ToUpper().Contains("TRAINEE"))
                    return true;
                return false;
            }
        }
        public bool is_correct_follower(int followers)
        {
            if (this.tier_level == 1)
            {
                return followers >= 10000;
            }
            else if (this.tier_level == 2)
            {
                return followers >= 3000 && followers <= 9999;
            }
            else if (this.tier_level == 3)
            {
                return followers >= 1000 && followers <= 2999;
            }
            else if (this.tier_level == 4)
            {
                return followers >= 500 && followers <= 999;
            }
            else if (this.tier_level == 5)
            {
                return followers >= 0 && followers <= 499;
            }
            return false;
        }
        public static Agent Get(string username)
        {
            using (var client = new HttpClient())
            {
                var uri = string.Concat(Url.API_URL, "/agent/?username=", username,"&version=", Globals.CurrentVersion());
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

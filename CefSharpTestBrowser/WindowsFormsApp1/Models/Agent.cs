using Newtonsoft.Json;
using CSTool.Class;
using System;
using System.Net.Http;
using System.Windows.Forms;
using CSTool.Properties;
using CSTool.Handlers;
using CSTool.Handlers.Interfaces;
using CSTool.Handlers.ErrorsHandler;

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
        public string room_type { get; set; }
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

        public static Agent Get(string username)
        {
            try
            {
                using (IHttpHandler client = new HttpHandler())
                {
                    var uri = string.Concat(Url.API_URL, "/agent/?username=", username, "&version=", Globals.CurrentVersion());
                    using (HttpResponseMessage response = client.CustomGetAsync(uri).Result)
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
                        else if (response.StatusCode == System.Net.HttpStatusCode.Gone)
                        {
                            MessageBox.Show("Invalid app version. Please update your application.", "Error");
                            Application.Exit();
                        }
                    }
                }
                return null;
            }
            catch (AggregateException e) when (e.InnerException is UnauthorizeException unauthorize)
            {
                Globals.SessionExpired();
                throw unauthorize;
            }
        }

        public string HumanizedRoomType()
        {
            switch (this.room_type)
            {
                case RoomType.ChatMedia:
                    return "Chat Media Room";
                case RoomType.Exhibitionist:
                    return "Exhibitionist Room";
                case RoomType.Photoset:
                    return "Photoset Room";
                case RoomType.NotificationPhotoset:
                    return "Notification Photoset Room";
                case RoomType.Chat:
                    return "Chat Room";
                default:
                    return "Compliance Room";
            }
        }
    }
}

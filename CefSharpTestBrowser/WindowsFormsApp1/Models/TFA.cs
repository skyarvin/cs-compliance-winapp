using CSTool.Handlers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using WindowsFormsApp1;
using CSTool.Handlers.Interfaces;
using CSTool.Class;

namespace CSTool.Models
{
    public class TFA: UserToken
    {
        public string user_id { get; set; }
        public string nonce { get; set; }
        public List<Dictionary<string, string>> devices { get; set; }

        public static bool SubmitTfa(string tfa_code, string nonce, string user_id, string device_name)
        {
            try
            {
                using (IHttpHandler client = new HttpHandler())
                {
                    var uri = string.Concat(Url.AUTH_URL, "/confirm_login");
                    client.Timeout = TimeSpan.FromSeconds(5);
                    var content = new StringContent(JsonConvert.SerializeObject(new
                    {
                        tfa_code,
                        nonce,
                        user_id,
                        device_name,
                    }), Encoding.UTF8, "application/json");
                    var response = client.CustomPostAsync(uri, content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        using (HttpContent data = response.Content)
                        {
                            var jsonString = data.ReadAsStringAsync();
                            jsonString.Wait();
                            UserToken token = JsonConvert.DeserializeObject<UserToken>(jsonString.Result);
                            Globals.UserToken = new UserToken
                            {
                                access_token = token.access_token,
                                refresh_token = token.refresh_token,
                            };
                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
    }
}

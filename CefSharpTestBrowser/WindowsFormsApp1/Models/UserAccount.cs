using CSTool.Handlers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Models;
using WindowsFormsApp1;
using CSTool.Class;
using System.Security.Cryptography;
using System.IO;
using System.Security.Principal;
using CSTool.Properties;
using CSTool.Handlers.Interfaces;
using CSTool.Handlers.ErrorsHandler;
using Newtonsoft.Json.Linq;

namespace CSTool.Models
{
    public class UserAccount
    {
        public string username;
        public string role;
        public static IMFAToken UserLogin(string username, string password)
        {
            try
            {
                using (IHttpHandler client = new HttpHandler())
                {
                    var uri = string.Concat(Url.AUTH_URL, "/login");
                    client.Timeout = TimeSpan.FromSeconds(5);
                    var content = new StringContent(JsonConvert.SerializeObject(new
                    {
                        username,
                        password
                    }), Encoding.UTF8, "application/json");
                    var response = client.CustomPostAsync(uri, content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        using (HttpContent data = response.Content)
                        {
                            var jsonString = data.ReadAsStringAsync();
                            jsonString.Wait();
                            IMFAToken result = JsonConvert.DeserializeObject<IMFAToken>(jsonString.Result);
                            if(result?.nonce == null)
                            {
                                Globals.UserToken = new UserToken
                                {
                                    access_token = result.access_token,
                                    refresh_token = result.refresh_token,
                                };
                            }
                            return result;
                        }
                    }
                    return null; 
                }
            }
            catch
            {
                return null; 
            }
        }

        public static bool UserLogout()
        {
            try
            {
                using (IHttpHandler client = new HttpHandler())
                {
                    var uri = string.Concat(Url.AUTH_URL, "/logout");
                    client.Timeout = TimeSpan.FromSeconds(5);
                    var response = client.CustomPostAsync(uri).Result;
                    return response.IsSuccessStatusCode;
                }
            }
            catch (AggregateException e) when (e.InnerException is UnauthorizeException)
            {
                Globals.SessionExpired();
                return false;
            }
        }
    }
}

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

namespace CSTool.Models
{
    public class UserAccount
    {
        //public int id { get; set; }
        //public string username { get; set; }
        //public string password { get; set; }

        /// <summary>
        /// API get to fetch account from  User pool
        /// </summary>
        /// <returns></returns>

        public bool UserLogin(string username, string password)
        {
            try
            {
                using (var client = new HttpHandler())
                {
                    var uri = string.Concat(Url.AUTH_URL, "/login");
                    client.Timeout = TimeSpan.FromSeconds(5);
                    var response = client.CPostAsync(uri, JsonConvert.SerializeObject(new
                    {
                        username,
                        password
                    }));
                    if (response.IsSuccessStatusCode)
                    {
                        using (HttpContent data = response.Content)
                        {
                            var jsonString = data.ReadAsStringAsync();
                            jsonString.Wait();
                            var tokens = JsonConvert.DeserializeObject<UserToken>(jsonString.Result);
                            Globals.UserToken = new UserToken
                            {
                                access_token = tokens.access_token,
                                refresh_token = tokens.refresh_token,
                            };
                            return true;
                        }
                    }
                    return false; 
                }
            }
            catch
            {
                return false;
            }
        }
    }
}

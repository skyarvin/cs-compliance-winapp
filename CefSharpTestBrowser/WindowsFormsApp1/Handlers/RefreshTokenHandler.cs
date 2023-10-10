using CefSharp;
using CefSharp.WinForms;
using CSTool.Class;
using CSTool.Handlers.ErrorsHandler;
using CSTool.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1;

namespace CSTool.Handlers
{
    internal class RefreshTokenHandler
    {
        readonly HttpClient request;

        public RefreshTokenHandler(HttpClient client)
        {
            this.request = client;
        }

        public bool RefreshToken()
        {
            try
            {
                const int MaxRetries = 3;
                lock (Globals.refreshLock)
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Add("Authorization", Globals.UserToken.refresh_token);
                        for (int i = 0; i < MaxRetries; i++)
                        {
                            using (HttpResponseMessage refreshResponse = client.PostAsync(Url.AUTH_URL + "/refresh", null).Result)
                            {
                                if (refreshResponse.IsSuccessStatusCode)
                                {
                                    using (HttpContent data = refreshResponse.Content)
                                    {
                                        var jsonString = data.ReadAsStringAsync();
                                        jsonString.Wait();
                                        UserToken tokens = JsonConvert.DeserializeObject<UserToken>(jsonString.Result);
                                        Globals.UserToken.access_token = tokens.access_token;
                                        Globals.UserToken.refresh_token = tokens.refresh_token;
                                        this.request.DefaultRequestHeaders.Remove("Authorization");
                                        this.request.DefaultRequestHeaders.Add("Authorization", Globals.UserToken.access_token);
                                    }
                                    return true;
                                }
                            }
                        }
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}

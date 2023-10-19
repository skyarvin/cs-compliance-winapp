﻿using CSTool.Handlers;
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
    public class TFA
    {
        public string tfa_code { get; set; }
        public string nonce { get; set; }
        public string user_id { get; set; }
        public string device_id { get; set; }

        public bool SubmitTfa()
        {
            try
            {
                using (IHttpHandler client = new HttpHandler())
                {
                    var uri = string.Concat(Url.AUTH_URL, "/tfa_code/");
                    client.Timeout = TimeSpan.FromSeconds(5);
                    var content = new StringContent(JsonConvert.SerializeObject(new
                    {
                        this.tfa_code,
                        this.nonce,
                        this.user_id,
                        this.device_id,
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

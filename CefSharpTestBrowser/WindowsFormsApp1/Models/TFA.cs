using CSTool.Handlers;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using WindowsFormsApp1;
using CSTool.Handlers.Interfaces;
using CSTool.Class;
using CSTool.Handlers.ErrorsHandler;

namespace CSTool.Models
{
    public class TFA
    {
        public string tfa_code { get; set; }
        public string nonce { get; set; }
        public string user_id { get; set; }
        public string device_id { get; set; }
        public string prev_device_id { get; set; }

        public void ValidateTfa(bool use_recovery_code = false)
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
                        use_recovery_code
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
                        }
                    }
                }
            }
            catch (AggregateException e) when (e.InnerException is UnauthorizeException unauthorize)
            {
                throw unauthorize;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string ChangeAuthenticatorDevice()
        {
            try
            {
                using (IHttpHandler client = new HttpHandler())
                {
                    var uri = string.Concat(Url.AUTH_URL, "/tfa/device/change/");
                    client.Timeout = TimeSpan.FromSeconds(5);
                    var content = new StringContent(JsonConvert.SerializeObject(new
                    {
                        this.nonce,
                        this.user_id,
                        this.device_id,
                        this.prev_device_id,
                    }), Encoding.UTF8, "application/json");
                    var response = client.CustomPostAsync(uri, content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        using (HttpContent data = response.Content)
                        {
                            var jsonString = data.ReadAsStringAsync();
                            jsonString.Wait();
                            UserTFA tfa = JsonConvert.DeserializeObject<UserTFA>(jsonString.Result);
                            return tfa.nonce;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ResendTfaCode()
        {
            try
            {
                using (IHttpHandler client = new HttpHandler())
                {
                    var uri = string.Concat(Url.AUTH_URL, "/tfa/resend/");
                    client.Timeout = TimeSpan.FromSeconds(5);
                    var content = new StringContent(JsonConvert.SerializeObject(new
                    {
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
                            UserTFA tfa = JsonConvert.DeserializeObject<UserTFA>(jsonString.Result);
                            return tfa.nonce;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            } catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public string ToggleTFAMethod(bool use_recovery_code)
        {
            Console.WriteLine($"Nonce: {this.nonce} || User id: {this.user_id} || Device id: {this.device_id}");
            try
            {
                using (IHttpHandler client = new HttpHandler())
                {
                    var uri = string.Concat(Url.AUTH_URL, "/tfa_code/toggle/");
                    client.Timeout = TimeSpan.FromSeconds(5);
                    var content = new StringContent(JsonConvert.SerializeObject(new
                    {
                        this.nonce,
                        this.user_id,
                        this.device_id,
                        use_recovery_code
                    }), Encoding.UTF8, "application/json");
                    var response = client.CustomPostAsync(uri, content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        using (HttpContent data = response.Content)
                        {
                            var jsonString = data.ReadAsStringAsync();
                            jsonString.Wait();
                            UserTFA tfa = JsonConvert.DeserializeObject<UserTFA>(jsonString.Result);
                            return tfa.nonce;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

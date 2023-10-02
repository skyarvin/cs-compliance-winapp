using CSTool.Class;
using CSTool.Handlers.Interfaces;
using CSTool.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1;

namespace CSTool.Handlers
{
    enum RequestType
    {
        Post,
        Get,
        Put,
    }
    internal class HttpHandler: HttpClient, IHttpHandler
    {
        public async Task<HttpResponseMessage> CustomGetAsync(string requestUri)
        {
            try
            {
                Uri uri = new Uri(requestUri);
                this.SetRequestHeaders(uri);
                HttpResponseMessage response = GetAsync(uri).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return await Task.FromResult(await HandleRefreshToken(requestUri, RequestType.Get));
                }
                return await Task.FromResult(response);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public async Task<HttpResponseMessage> CustomPostAsync(string requestUri, HttpContent content)
        {
            try
            {
                Uri uri = new Uri(requestUri);
                this.SetRequestHeaders(uri);
                var body = content.ReadAsStringAsync().Result;
                HttpResponseMessage response = PostAsync(requestUri, content).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return await Task.FromResult(await HandleRefreshToken(requestUri, RequestType.Post, body));
                }
                return await Task.FromResult(response);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public async Task<HttpResponseMessage> CustomPutAsync(string requestUri, HttpContent content)
        {
            try
            {
                Uri uri = new Uri(requestUri);
                this.SetRequestHeaders(uri);
                var body = content.ReadAsStringAsync().Result;
                HttpResponseMessage response = PutAsync(requestUri, content).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return await Task.FromResult(await HandleRefreshToken(requestUri, RequestType.Put, body));
                }
                return await Task.FromResult(response);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void SetRequestHeaders(Uri uri)
        {
            string[] public_routes = { "/security/login" };
            DefaultRequestHeaders.Add("Authorization", Globals.apiKey);
            if (!public_routes.Contains(uri.AbsolutePath))
            {
                DefaultRequestHeaders.Add("Staffme-Authorization", Globals.UserToken.access_token);
            }
        }

        private Task<HttpResponseMessage> HandleRefreshToken(string requestUri, RequestType requestType, string body = null)
        {
            try
            {
                object _lock = new object();
                lock (_lock)
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Add("Staffme-Authorization", Globals.UserToken.refresh_token);
                        using (HttpResponseMessage refreshResponse = client.PostAsync(Url.AUTH_URL + "/refresh", null).Result)
                        {
                            if (!refreshResponse.IsSuccessStatusCode)
                            {
                                throw new Exception("Failed to refresh token.");
                            }
                            using (HttpContent data = refreshResponse.Content)
                            {
                                var jsonString = data.ReadAsStringAsync();
                                jsonString.Wait();
                                UserToken tokens = JsonConvert.DeserializeObject<UserToken>(jsonString.Result);
                                Globals.UserToken.access_token = tokens.access_token;
                                Globals.UserToken.refresh_token = tokens.refresh_token;
                                DefaultRequestHeaders.Remove("Staffme-Authorization");
                                DefaultRequestHeaders.Add("Staffme-Authorization", Globals.UserToken.access_token);
                            }
                        }
                    }
                }

                HttpResponseMessage response = null;
                switch (requestType)
                {
                    case RequestType.Post:
                        var postContent = new StringContent(body, Encoding.UTF8, "application/json");
                        response = PostAsync(requestUri, postContent).Result;
                        break;
                    case RequestType.Put:
                        var putContent = new StringContent(body, Encoding.UTF8, "application/json");
                        response = PutAsync(requestUri, putContent).Result;
                        break;
                    case RequestType.Get:
                        response = GetAsync(requestUri).Result;
                        break;
                }
                return Task.FromResult(response);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}

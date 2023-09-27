using CSTool.Class;
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
    internal class HttpHandler: HttpClient
    {
        private static SemaphoreSlim sem = new SemaphoreSlim(1);
        string[] public_routes = { "/security/login" };
        public HttpResponseMessage CGetAsync(string requestUri)
        {
            try
            {
                Uri uri = new Uri(requestUri);
                this.SetRequestHeaders(uri);
                HttpResponseMessage response = GetAsync(uri).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return this.HandleRefreshToken(requestUri, RequestType.Get).Result;
                }
                return response;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public HttpResponseMessage CPostAsync(string requestUri, string body)
        {
            try
            {
                Uri uri = new Uri(requestUri);
                this.SetRequestHeaders(uri);
                HttpContent content = new StringContent((body), Encoding.UTF8, "application/json");
                HttpResponseMessage response = PostAsync(requestUri, content).Result;
                if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return this.HandleRefreshToken(requestUri, RequestType.Post, body).Result;
                }
                return response;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        private void SetRequestHeaders(Uri uri)
        {
            DefaultRequestHeaders.Add("Authorization", Globals.apiKey);
            if (!public_routes.Contains(uri.AbsolutePath))
            {
                DefaultRequestHeaders.Add("Staffme-Authorization", Globals.UserToken.access_token);
            }
        }

        public async Task<HttpResponseMessage> HandleRefreshToken(string requestUri, RequestType requestType, string body = null)
        {
            try
            {
                await sem.WaitAsync();
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
                sem.Dispose();

                HttpResponseMessage response = null;
                HttpContent content = new StringContent((body), Encoding.UTF8, "application/json");
                switch (requestType)
                {
                    case RequestType.Post:
                        response = PostAsync(requestUri, content).Result;
                        break;
                    case RequestType.Get:
                        response = GetAsync(requestUri).Result;
                        break;
                }
                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}

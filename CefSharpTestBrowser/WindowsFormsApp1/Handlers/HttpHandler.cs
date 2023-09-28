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
        private readonly object _lock;
        string[] public_routes = { "/security/login" };

        public HttpHandler()
        {
            this._lock = new object();
        }

        public async Task<HttpResponseMessage> CGetAsync(string requestUri)
        {
            try
            {
                Uri uri = new Uri(requestUri);
                this.SetRequestHeaders(uri);
                HttpResponseMessage response = GetAsync(uri).Result;
                Console.WriteLine(requestUri + " " + response);
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return await Task.FromResult(await HandleRefreshToken(requestUri, RequestType.Get));
                }
                return await Task.FromResult(response);
            }
            catch(Exception e)
            {
                Console.WriteLine("TESTTEST: " + e);
                throw e;
            }
        }

        public async Task<HttpResponseMessage> CPostAsync(string requestUri, string body)
        {
            try
            {
                Uri uri = new Uri(requestUri);
                this.SetRequestHeaders(uri);
                HttpContent content = new StringContent((body), Encoding.UTF8, "application/json");
                HttpResponseMessage response = PostAsync(requestUri, content).Result;
                if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
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

        private void SetRequestHeaders(Uri uri)
        {
            DefaultRequestHeaders.Add("Authorization", Globals.apiKey);
            if (!public_routes.Contains(uri.AbsolutePath))
            {
                DefaultRequestHeaders.Add("Staffme-Authorization", Globals.UserToken.access_token);
            }
        }

        public Task<HttpResponseMessage> HandleRefreshToken(string requestUri, RequestType requestType, string body = null)
        {
            try
            {
                lock (_lock)
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Add("Staffme-Authorization", Globals.UserToken.refresh_token);
                        using (HttpResponseMessage refreshResponse = client.PostAsync(Url.AUTH_URL + "/refresh", null).Result)
                        {
                            Console.WriteLine("Refresh " + requestUri + " " + refreshResponse);
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
                        HttpContent content = new StringContent((body), Encoding.UTF8, "application/json");
                        Console.WriteLine("HEADERS: POST " + DefaultRequestHeaders);
                        response = PostAsync(requestUri, content).Result;
                        break;
                    case RequestType.Get:
                        Console.WriteLine("HEADERS: GET " + DefaultRequestHeaders);
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

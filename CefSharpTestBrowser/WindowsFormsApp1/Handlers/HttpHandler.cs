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
                DefaultRequestHeaders.Add("Authorization", Globals.apiKey);
                Uri uri = new Uri(requestUri);
                this.CheckPublicRoute(uri);
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

        public HttpResponseMessage CPostAsync(string requestUri, HttpContent content)
        {
            try
            {
                DefaultRequestHeaders.Add("Authorization", Globals.apiKey);
                Uri uri = new Uri(requestUri);
                this.CheckPublicRoute(uri);
                HttpResponseMessage response = PostAsync(requestUri, content).Result;
                if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return this.HandleRefreshToken(requestUri, RequestType.Post, content).Result;
                }
                return response;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        private void CheckPublicRoute(Uri uri)
        {
            if (!public_routes.Contains(uri.AbsolutePath))
            {
                DefaultRequestHeaders.Add("Staffme-Authorization", Globals.UserToken.access_token);
            }
        }

        private async Task<HttpResponseMessage> HandleRefreshToken(string requestUri, RequestType requestType, HttpContent content = null)
        {
            try
            {
                await sem.WaitAsync();
                HttpRequestMessage TokenRequest = new HttpRequestMessage(HttpMethod.Post, new Uri(new Uri(Url.API_URL), "Token"));
                TokenRequest.Headers.Add("Staffme-Authorization", Globals.UserToken.refresh_token);
                using (HttpResponseMessage refreshResponse = PostAsync(new Uri(new Uri(Url.AUTH_URL), "refresh"), new StringContent("", Encoding.UTF8, "application/json")).Result)
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
                        DefaultRequestHeaders.Remove("Staffme-Authorization");
                        DefaultRequestHeaders.Add("Staffme-Authorization", Globals.UserToken.access_token);
                    }
                }
                sem.Dispose();

                HttpResponseMessage response = null;
                switch (requestType)
                {
                    case RequestType.Post:
                        response = CPostAsync(requestUri, content);
                        break;
                    case RequestType.Get:
                        response = CGetAsync(requestUri);
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

using CSTool.Class;
using CSTool.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowsFormsApp1;

namespace CSTool.Handlers
{
    internal class HttpHandler: HttpClient
    {
        private static SemaphoreSlim sem = new SemaphoreSlim(1);
        string[] private_routes = {"/api/agent/"};
        public HttpResponseMessage CGetAsync(string requestUri)
        {
            try
            {
                DefaultRequestHeaders.Add("Authorization", Globals.apiKey);
                Uri uri = new Uri(requestUri);
                this.CheckPrivateRoute(uri);
                var response = GetAsync(uri).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return this.HandleUnAuthorizedResponse(requestUri, "get").Result;
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
                this.CheckPrivateRoute(uri);
                var response = PostAsync(requestUri, content).Result;
                if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return this.HandleUnAuthorizedResponse(requestUri, "post", content).Result;
                }
                return response;
            }catch(Exception e)
            {
                throw e;
            }
        }

        private void CheckPrivateRoute(Uri uri)
        {
            if (private_routes.Contains(uri.AbsolutePath))
            {
                DefaultRequestHeaders.Add("Staffme-Authorization", Globals.UserToken.access_token);
            }
        }

        private async Task<HttpResponseMessage> HandleUnAuthorizedResponse(string requestUri, string requestType, HttpContent content = null)
        {
            try
            {
                await sem.WaitAsync();
                HttpRequestMessage TokenRequest = new HttpRequestMessage(HttpMethod.Post, new Uri(new Uri(Url.API_URL), "Token"));
                TokenRequest.Headers.Add("Staffme-Authorization", "test");
                HttpContent content1 = new StringContent("", Encoding.UTF8, "application/json");
                using (var refreshResponse = PostAsync(new Uri(new Uri(Url.API_URL), "token"), content1).Result)
                {
                    if (!refreshResponse.IsSuccessStatusCode)
                    {
                        using (HttpContent data = refreshResponse.Content)
                        {
                            var jsonString = data.ReadAsStringAsync();
                            jsonString.Wait();
                            var tokens = JsonConvert.DeserializeObject<UserToken>(jsonString.Result);
                            Globals.UserToken.access_token = tokens.access_token;
                            DefaultRequestHeaders.Remove("Staffme-Authorization");
                            DefaultRequestHeaders.Add("Staffme-Authorization", Globals.UserToken.access_token);
                        }
                    }
                }
                sem.Dispose();

                HttpResponseMessage response = null;
                switch (requestType)
                {
                    case "post":
                        response = CPostAsync(requestUri, content);
                        break;
                    case "get":
                        response = CGetAsync(requestUri);
                        break;
                }
                return response;
            }catch (Exception e)
            {
                throw e;
            }

        }
    }
}

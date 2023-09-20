using CefSharp;
using CSTool.Class;
using CSTool.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using WindowsFormsApp1;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CSTool.Handlers
{
    internal class HttpHandler: HttpClient
    {
        private static SemaphoreSlim sem = new SemaphoreSlim(1);
        string[] private_routes = {"/api/agent/"};
        //public override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        //{
        //    request.Headers.Add("Authorization", Globals.apiKey);
        //    Uri myUri = new Uri(request.RequestUri.ToString());
        //    if (private_routes.Contains(myUri.AbsolutePath))
        //    {
        //        request.Headers.Add("Staffme-Authorization", Settings.Default.access_token);
        //    }
        //    Console.WriteLine(request.Method + " " + request.RequestUri + "\n" + request.Headers);
        //    var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        //    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        //    {
        //        //this.HandleUnAuthorizedResponse(request, cancellationToken);
        //    }
        //    return response;
        //}

        public HttpResponseMessage CGetAsync(string requestUri)
        {
            try
            {
                DefaultRequestHeaders.Add("Authorization", Globals.apiKey);
                Uri uri = new Uri(requestUri);
                this.CheckPrivateRoute(uri);
                var response = GetAsync(uri).Result;
                Console.WriteLine(response.StatusCode);

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
                Console.WriteLine(response.StatusCode);

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
                DefaultRequestHeaders.Add("Staffme-Authorization", Settings.Default.access_token);
            }
        }

        private async Task<HttpResponseMessage> HandleUnAuthorizedResponse(string requestUri, string type, HttpContent content = null)
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
                        return refreshResponse;
                    }
                    DefaultRequestHeaders.Remove("Staffme-Authorization");
                    DefaultRequestHeaders.Add("Staffme-Authorization", "MyCustomValue");
                }
                sem.Dispose();

                HttpResponseMessage response = null;
                switch (type)
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

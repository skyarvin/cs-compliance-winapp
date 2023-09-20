using CSTool.Class;
using CSTool.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1;

namespace CSTool.Handlers
{
    internal class HttpHandler: HttpClientHandler
    {
        private static SemaphoreSlim sem = new SemaphoreSlim(1);
        string[] private_routes = {"/api/agent/"};
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("Authorization", Globals.apiKey);
            Uri myUri = new Uri(request.RequestUri.ToString());
            if (private_routes.Contains(myUri.AbsolutePath))
            {
                request.Headers.Add("Staffme-Authorization", Settings.Default.access_token);
            }
            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                //this.HandleUnAuthorizedResponse(request, cancellationToken);
            }
            return response;
        }

        private async void HandleUnAuthorizedResponse(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await sem.WaitAsync();
            HttpRequestMessage TokenRequest = new HttpRequestMessage(HttpMethod.Post, new Uri(new Uri(Url.API_URL), "Token"));
            TokenRequest.Headers.Add("Staffme-Authorization", "test");
            using (var refreshResponse = await base.SendAsync(TokenRequest, cancellationToken))
            {
                if (!refreshResponse.IsSuccessStatusCode)
                {
                    MessageBox.Show("User not authorized!");
                    return;
                }
                request.Headers.Remove("Staffme-Authorization");
                request.Headers.Add("Staffme-Authorization", "MyCustomValue");
            }
            sem.Dispose();
        }
    }
}

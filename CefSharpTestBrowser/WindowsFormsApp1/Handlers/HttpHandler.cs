using CefSharp;
using CSTool.Class;
using CSTool.Handlers.ErrorsHandler;
using CSTool.Handlers.Interfaces;
using CSTool.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
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
                this.CheckTokenValidity();
                this.SetRequestHeaders(requestUri);
                HttpResponseMessage response = GetAsync(requestUri).Result;
                this.CheckRequestStatusCode(response.StatusCode);
                return await Task.FromResult(response);
            }
            catch (Exception e)
            {
                Globals.SaveToLogFile(e.ToString(), (int)LogType.Error);
                throw e;
            }
        }

        public async Task<HttpResponseMessage> CustomPostAsync(string requestUri, HttpContent content=null)
        {
            try
            {
                this.CheckTokenValidity();
                this.SetRequestHeaders(requestUri);
                HttpResponseMessage response = PostAsync(requestUri, content).Result;
                this.CheckRequestStatusCode(response.StatusCode);
                return await Task.FromResult(response);
            }
            catch (Exception e)
            {
                Globals.SaveToLogFile(e.ToString(), (int)LogType.Error);
                throw e;
            }
        }

        public async Task<HttpResponseMessage> CustomPutAsync(string requestUri, HttpContent content=null)
        {
            try
            {
                this.CheckTokenValidity();
                this.SetRequestHeaders(requestUri);
                HttpResponseMessage response = PutAsync(requestUri, content).Result;
                this.CheckRequestStatusCode(response.StatusCode);
                return await Task.FromResult(response);
            }
            catch (Exception e)
            {
                Globals.SaveToLogFile(e.ToString(), (int)LogType.Error);
                throw e;
            }
        }

        private void CheckRequestStatusCode(System.Net.HttpStatusCode statusCode)
        {
            if (statusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizeException();
            }
        }

        private void CheckTokenValidity()
        {
            lock (Globals.sharedRequestLock)
            {
                if (TokenHandler.shouldRefresh())
                {
                    new RefreshTokenHandler().RefreshToken();
                }
            }
        }

        private void SetRequestHeaders(string requestUri)
        {
            string[] public_routes = { "/security/login/", "/security/tfa_code/", "/security/tfa/device/change/" };
            DefaultRequestHeaders.Add("Staffme-Authorization", Globals.apiKey);
            if (!public_routes.Contains(new Uri(requestUri).AbsolutePath))
            {
                DefaultRequestHeaders.Add("Authorization", Globals.UserToken.access_token);
            }
        }
    }
}

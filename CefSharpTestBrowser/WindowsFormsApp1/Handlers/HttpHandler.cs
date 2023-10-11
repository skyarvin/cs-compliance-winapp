﻿using CSTool.Class;
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
                this.SetRequestHeaders(requestUri);
                HttpResponseMessage response = GetAsync(requestUri).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return await Task.FromResult(await new RefreshTokenHandler(this).RetryActionHandler(requestUri, RequestType.Get));
                }
                return await Task.FromResult(response);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<HttpResponseMessage> CustomPostAsync(string requestUri, HttpContent content=null)
        {
            try
            {
                this.SetRequestHeaders(requestUri);
                var body = content?.ReadAsStringAsync().Result;
                HttpResponseMessage response = PostAsync(requestUri, content).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return await Task.FromResult(await new RefreshTokenHandler(this).RetryActionHandler(requestUri, RequestType.Post, body));
                }
                return await Task.FromResult(response);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<HttpResponseMessage> CustomPutAsync(string requestUri, HttpContent content=null)
        {
            try
            {
                this.SetRequestHeaders(requestUri);
                var body = content?.ReadAsStringAsync().Result;
                HttpResponseMessage response = PutAsync(requestUri, content).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return await Task.FromResult(await new RefreshTokenHandler(this).RetryActionHandler(requestUri, RequestType.Put, body));
                }
                return await Task.FromResult(response);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void SetRequestHeaders(string requestUri)
        {
            string apiKey = "0a36fe1f051303b2029b25fd7a699cfcafb8e4619ddc10657ef8b32ba159e674";
            string[] public_routes = { "/security/login", "/security/confirm_login" };
            DefaultRequestHeaders.Add("Staffme-Authorization", apiKey);
            if (!public_routes.Contains(new Uri(requestUri).AbsolutePath))
            {
                DefaultRequestHeaders.Add("Authorization", Globals.UserToken.access_token);
            }
        }
    }
}

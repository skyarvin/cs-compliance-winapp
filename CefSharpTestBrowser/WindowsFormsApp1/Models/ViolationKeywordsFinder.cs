using CSTool.Handlers;
using CSTool.Handlers.Interfaces;
using System;
using System.Net.Http;

namespace CSTool.Models
{
    internal class ViolationKeywordsFinder
    {
        public static Tuple<bool, string> FetchViolationKeywords()
        {
            using (IHttpHandler client = new HttpHandler())
            {
                var uri = string.Concat(Class.Url.API_URL, "/fetch_violation_list");
                var response = client.CustomGetAsync(uri).Result;

                HttpContent data = response.Content;
                var jsonString = data.ReadAsStringAsync();
                jsonString.Wait();

                return Tuple.Create(response.IsSuccessStatusCode, jsonString.Result);
            }
        }
    }
}
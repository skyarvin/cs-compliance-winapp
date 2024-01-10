using CSTool.Handlers;
using System.Net.Http;
using CSTool.Handlers.Interfaces;

namespace CSTool.Models
{
    internal class ViolationKeywords
    {
        public static string FetchViolationKeywords()
        {
            using (IHttpHandler client = new HttpHandler())
            {
                try
                {
                    var uri = string.Concat(Class.Url.API_URL, "/fetch_violation_list");
                    var response = client.CustomGetAsync(uri).Result;

                    HttpContent data = response.Content;
                    var jsonString = data.ReadAsStringAsync();
                    jsonString.Wait();

                    return jsonString.Result;
                }
                catch
                {
                    return "";
                }
            }
        }
    }
}
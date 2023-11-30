using CSTool.Handlers;
using CSTool.Handlers.Interfaces;
using System;
using System.Net.Http;

namespace CSTool.Models
{
    internal class ViolationRequest
    {
        private static HttpResponseMessage response;
        public static bool IsFetchSuccess()
        {
            using (IHttpHandler client = new HttpHandler())
            {
                var uri = string.Concat(Class.Url.API_URL, "/fetch_violation_list");
                response = client.CustomGetAsync(uri).Result;
                return response.IsSuccessStatusCode;
            }
        }

        public static string GetViolationList()
        {
            using (HttpContent data = response.Content)
            {
                var jsonString = data.ReadAsStringAsync();
                jsonString.Wait();
                return jsonString.Result;
            }
        }

        public static String GetHighlightScript()
        {
            return @"
                const violation_list = REPLACE_WITH_LIST;
                const regex_pattern = new RegExp(`${violation_list.data.join('|')}`,'ig');
                let text = '';

                function highlight_text(){
                    if(violation_list.data.length){
                        text = $('#chatlog_user .chatlog_message');
                        for(let x = 0; x < text.length; x++){
                            text[x].innerHTML = text[x].innerText.replace(regex_pattern, match => `<span style='background-color:red; color:white'>${match}</span>`)
                        }  
                    }
                }
            ";
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1;
using CSTool.Class;
using CSTool.Handlers;
using CSTool.Handlers.Interfaces;
using CSTool.Handlers.ErrorsHandler;

namespace CSTool.Models
{
    public class InternalRequestFacePhoto
    {
        public int id { get; set; }
        public string url { get; set; }
        public int agent_id { get; set; }
        public string status { get; set; }
        public string reviewer_notes { get; set; }
        public string updated_at { get; set; }
        public int duration { get; set; }


        public InternalRequestFacePhoto Save()
        {
            try
            {
                Globals.SaveToLogFile(string.Concat("Save IRFP: ", JsonConvert.SerializeObject(this)), (int)LogType.Action);
                using (IHttpHandler client = new HttpHandler())
                {
                    var uri = string.Concat(Url.API_URL, "/irfp/");
                    client.Timeout = TimeSpan.FromSeconds(5);
                    var content = new StringContent(JsonConvert.SerializeObject(this), Encoding.UTF8, "application/json");
                    var response = client.CustomPostAsync(uri, content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        using (HttpContent data = response.Content)
                        {
                            var jsonString = data.ReadAsStringAsync();
                            jsonString.Wait();
                            return JsonConvert.DeserializeObject<InternalRequestFacePhoto>(jsonString.Result);
                        }
                    }
                    else
                    {
                        Globals.SaveToLogFile(JsonConvert.SerializeObject(this), (int)LogType.Error);
                        throw new Exception("Api save irfp request error, Please contact dev team");
                    }
                }
            }
            catch (AggregateException e) when (e.InnerException is UnauthorizeException)
            {
                Globals.redirect_to_login(e);
                throw e;
            }
        }

        public static InternalRequestFacePhoto Get(int irfp_id, int agent_id)
        {
            try
            {
                using (IHttpHandler client = new HttpHandler())
                {
                    var uri = string.Concat(Url.API_URL, "/irfp/", irfp_id, "/", agent_id, "/");
                    using (HttpResponseMessage response = client.CustomGetAsync(uri).Result)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            using (HttpContent content = response.Content)
                            {
                                var jsonString = content.ReadAsStringAsync();
                                jsonString.Wait();
                                return JsonConvert.DeserializeObject<InternalRequestFacePhoto>(jsonString.Result);
                            }
                        }
                    }
                }
                return null;
            }
            catch (AggregateException e) when (e.InnerException is UnauthorizeException)
            {
                Globals.redirect_to_login(e);
                throw e;
            }
        }

        public static InternalRequestFacePhoto GetAgentIRFP(int agent_id)
        {
            try
            {
                using (IHttpHandler client = new HttpHandler())
                {
                    var uri = string.Concat(Url.API_URL, "/irfp/agent/", agent_id, "/");
                    using (HttpResponseMessage response = client.CustomGetAsync(uri).Result)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            using (HttpContent content = response.Content)
                            {
                                var jsonString = content.ReadAsStringAsync();
                                jsonString.Wait();
                                return JsonConvert.DeserializeObject<InternalRequestFacePhoto>(jsonString.Result);
                            }
                        }
                    }
                }
                return null;
            }
            catch (AggregateException e) when (e.InnerException is UnauthorizeException)
            {
                Globals.redirect_to_login(e);
                throw e;
            }
        }
    }
}

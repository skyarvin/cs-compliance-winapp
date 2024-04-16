using CSTool.Class;
using CSTool.Handlers;
using CSTool.Handlers.Interfaces;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Net.Http;

namespace CSTool.Models
{
    public class InternalReview
    {
        public InternalIdentificationChecker iidc_info { get; set; }
        public InternalRequestFacePhoto irfp_info { get; set; }

        public static InternalReview GetInternalReviewInfo()
        {
            using (IHttpHandler client = new HttpHandler())
            {
                var uri = string.Concat(Url.API_URL, "/internal-review/info/");
                using (HttpResponseMessage response = client.CustomGetAsync(uri).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        using (HttpContent content = response.Content)
                        {
                            var jsonString = content.ReadAsStringAsync();
                            jsonString.Wait();
                            return JsonConvert.DeserializeObject<InternalReview>(jsonString.Result);
                        }
                    }
                }
            }
            return null;
        }
    }
}

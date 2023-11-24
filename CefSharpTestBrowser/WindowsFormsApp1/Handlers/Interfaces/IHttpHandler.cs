using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CSTool.Handlers.Interfaces
{
    internal interface IHttpHandler: IDisposable
    {
        Task<HttpResponseMessage> CustomGetAsync(string requestUri);
        Task<HttpResponseMessage> CustomPostAsync(string requestUri, HttpContent content=null);
        Task<HttpResponseMessage> CustomPutAsync(string requestUri, HttpContent content=null);
        TimeSpan Timeout { get; set; }
    }
}

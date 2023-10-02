using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CSTool.Handlers.Interfaces
{
    internal interface IHttpHandler: IDisposable
    {
        Task<HttpResponseMessage> CustomGetAsync(string requestUri);
        Task<HttpResponseMessage> CustomPostAsync(string requestUri, HttpContent content);
        Task<HttpResponseMessage> CustomPutAsync(string requestUri, HttpContent content);
        TimeSpan Timeout { get; set; }
    }
}

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
        Task<HttpResponseMessage> CGetAsync(string requestUri);
        Task<HttpResponseMessage> CPostAsync(string requestUri, HttpContent content);
        Task<HttpResponseMessage> CPutAsync(string requestUri, HttpContent content);
        TimeSpan Timeout { get; set; }
    }
}

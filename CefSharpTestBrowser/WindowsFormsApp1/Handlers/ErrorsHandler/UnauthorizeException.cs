using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CSTool.Handlers.ErrorsHandler
{
    internal class UnauthorizeException: Exception
    {
        public HttpResponseMessage response;
        public UnauthorizeException(): base("Invalid token") { }
        public UnauthorizeException(string message): base(message) { }
        public UnauthorizeException(HttpResponseMessage response): base() { 
            this.response = response;
        }
    }
}

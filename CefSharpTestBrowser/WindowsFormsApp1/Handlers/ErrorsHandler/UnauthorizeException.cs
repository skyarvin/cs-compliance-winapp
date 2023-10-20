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
        public HttpContent responseContent;
        public UnauthorizeException() : base("Invalid token") { }
        public UnauthorizeException(string message) : base(message) { }
        public UnauthorizeException(HttpContent responseContent) : base() { 
            this.responseContent = responseContent;
        }
    }
}

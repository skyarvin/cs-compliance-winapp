using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CSTool.Handlers.ErrorsHandler
{
    internal class ForbiddenException : Exception
    {
        public HttpContent responseContent;
        public ForbiddenException() : base("Invalid action") { }
        public ForbiddenException(string message) : base(message) { }
        public ForbiddenException(HttpContent responseContent) : base()
        {
            this.responseContent = responseContent;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSTool.Handlers.ErrorsHandler
{
    internal class UnauthorizeException: Exception
    {
        public UnauthorizeException(): base("Invalid token") { }
        public UnauthorizeException(string message): base(message) { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSTool.Handlers.ErrorsHandler
{
    public class TFARequiredException: Exception
    {
        public TFARequiredException() : base("TFA Required") { }
        public TFARequiredException(string message) : base(message) { }
    }
}

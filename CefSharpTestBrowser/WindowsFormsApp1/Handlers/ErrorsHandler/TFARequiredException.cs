using CSTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSTool.Handlers.ErrorsHandler
{
    public class TFARequiredException: Exception
    {
        public UserTFA userTfa;
        public TFARequiredException(string message) : base(message) { }
        public TFARequiredException(UserTFA userTfa) : base("TFA Required") {
            this.userTfa = userTfa;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSTool.Handlers.ErrorsHandler
{
    public class RegisterDeviceException : Exception
    {
        public string nonce { get; set; }
        public string user_id { get; set; }

        public RegisterDeviceException(string nonce, string user_id) : base("Device registration required")
        {
            this.nonce = nonce;
            this.user_id = user_id;
        }
    }
}

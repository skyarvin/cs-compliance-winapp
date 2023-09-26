using CSTool.Handlers;
using CSTool.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSTool.Models
{
    internal class UserToken
    {
        public string access_token { get; set; }

        public string refresh_token { get; set; }
    }
}

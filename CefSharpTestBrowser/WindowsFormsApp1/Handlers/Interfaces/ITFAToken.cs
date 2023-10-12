using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSTool.Handlers.Interfaces
{
    public class ITFAToken
    {
        public string nonce { get; set; }
        public string user_id { get; set; }
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public List<Dictionary<string, string>> devices { get; set; }
    }
}

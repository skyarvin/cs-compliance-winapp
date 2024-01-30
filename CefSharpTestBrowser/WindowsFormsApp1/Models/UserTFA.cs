using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSTool.Models
{
    public class UserTFA
    {
        public string nonce { get; set; }
        public string user_id { get; set; }
        public List<Dictionary<string, string>> devices { get; set; }
    }
}

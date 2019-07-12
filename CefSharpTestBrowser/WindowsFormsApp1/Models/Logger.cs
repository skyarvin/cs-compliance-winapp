using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Models
{
    public class Logger
    {
        public int id { get; set; }
        public string agent_id { get; set; }
        public string url { get; set; }
        public string action { get; set; }
        public string remarks { get; set; }
    }

    public class Agent {
        public int id { get; set; }
        public string  name { get; set; }
        public string email { get; set; }
    }
}

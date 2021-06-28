using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSTool.Models
{
    public class ErrorLogs
    {
        public int ID { get; set; }
        public string Data { get; set; }
        public string Action { get; set; }
        public int Sent { get; set; }
        public string Created_at { get; set; }
    }
}

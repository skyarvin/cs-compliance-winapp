using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1;

namespace SkydevCSTool.Class
{
    public class PairCommand
    {
        public int Timestamp { get; set; }
        public string Message { get; set; }
        public string Action { get; set; }
        public string Profile { get; set; }
    }
}

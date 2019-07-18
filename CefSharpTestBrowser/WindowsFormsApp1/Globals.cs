using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1
{
    static class Globals
    {
        public static Agent ComplianceAgent = new Agent();
    }

    public enum LogType:int
    {
        Action = 1,
        Url_Change = 2,
        Error = 3,
    }
}

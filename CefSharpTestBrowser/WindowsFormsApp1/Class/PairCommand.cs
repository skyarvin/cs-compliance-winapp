using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1;

namespace CSTool.Class
{
    public class PairCommand
    {
        public int RoomDuration { get; set; }
        public string Message { get; set; }
        public string Action { get; set; }
        public string Profile { get; set; }
        public int ProfileID { get; set; }

        public string Url { get; set; }
        public int NumberofActiveProfiles { get; set; }
        public string Preference { get; set; }
    }

    public class Profile
    {
        public string Name { get; set; }
        public string RemoteAddress { get; set; }
        public int AgentID { get; set; }
        public string Type { get; set; }
        public string Preference { get; set; }
        public bool IsActive { get; set; }
    }
}

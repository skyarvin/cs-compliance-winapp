using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Models
{
    public class Action
    {
        public bool MyProperty { get; set; }

        public bool VIOLATION_OF_RULES(string id)
        {
            if (id == "rules_button") {
                return true;
            }
            return false;
        }
    }
}

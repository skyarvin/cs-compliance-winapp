using SkydevCSTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1
{
    static class Globals
    {
        public static Agent ComplianceAgent = new Agent();
        public static string CB_COMPLIANCE_URL = "https://chaturbate.com/compliance";
        public static string CB_COMPLIANCE_SET_ID_EXP_URL = "https://chaturbate.com/compliance/update_expiration_date_form";
        public static string GOOGLE_TRANSLATE_URL = "https://translate.google.com/#view=home&op=translate&sl=auto&tl=en&text=";
        public static int LastSuccessId;
        public static DateTime StartTime = DateTime.Now;
        private static frmMessage frm = new frmMessage();
        public static void ShowMessage(Form parent)
        {
            if (frm.Visible != true){
                frm.ShowDialog(parent);
            }
           
        }
    }

    public enum LogType:int
    {
        Action = 1,
        Url_Change = 2,
        Error = 3,
        Activity = 4,
    }

    
}

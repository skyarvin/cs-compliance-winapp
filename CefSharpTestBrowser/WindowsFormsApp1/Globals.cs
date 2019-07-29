using SkydevCSTool;
using SkydevCSTool.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1
{
    static class Globals
    {
        public static Agent ComplianceAgent = new Agent();
        public static Activity activity = new Activity();
        public static string CB_COMPLIANCE_URL = "https://chaturbate.com/compliance";
        public static string CB_COMPLIANCE_SET_ID_EXP_URL = "https://chaturbate.com/compliance/update_expiration_date_form";
        public static string GOOGLE_TRANSLATE_URL = "https://translate.google.com/#view=home&op=translate&sl=auto&tl=en&text=";
        public static string baseUrl = "https://cscb.skydev.solutions/api";
        public static string apiKey = "0a36fe1f051303b2029b25fd7a699cfcafb8e4619ddc10657ef8b32ba159e674";
        public static int LAST_SUCCESS_ID;
        public static int FIVE_MINUTES_IDLE_TIME = 300000;
        public static DateTime _wentIdle;
        public static int _idleTicks;

        private static frmMessage frm = new frmMessage();
        public static void ShowMessage(Form parent)
        {
            if (frm.Visible != true){
                frm.ShowDialog(parent);
            }
           
        }
        public static string myStr(object o, string label = "")
        {
            try
            {
                if (o.ToString() != "--" && !string.IsNullOrEmpty(o.ToString()))
                    return string.Concat(label, o.ToString(), System.Environment.NewLine, " ");
                else
                    return "";
            }
            catch
            {
                return "";
            }
        }

        public static void SaveToLogFile(string logText, int logtype)
        {
            string logFilePath = "";
            string path = String.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "/SkydevCsTool/logs/");
            switch (logtype)
            {
                case (int)LogType.Action:
                    logFilePath = @path + "log.txt";
                    break;
                case (int)LogType.Url_Change:
                    logFilePath = @path + "url_log.txt";
                    break;
                case (int)LogType.Error:
                    logFilePath = @path + "error_log.txt";
                    break;
                case (int)LogType.Activity:
                    logFilePath = @path + "activity_log.txt";
                    break;
            }

            FileInfo logFileInfo = new FileInfo(logFilePath);
            DirectoryInfo logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
            if (!logDirInfo.Exists) logDirInfo.Create();
            using (FileStream fileStream = new FileStream(logFilePath, FileMode.Append))
            {
                using (StreamWriter log = new StreamWriter(fileStream))
                {
                    log.WriteLine(DateTime.Now.ToString());
                    log.WriteLine(logText);
                    log.Write(System.Environment.NewLine);
                }
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

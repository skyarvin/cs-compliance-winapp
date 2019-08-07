using CefSharp.WinForms;
using SkydevCSTool;
using SkydevCSTool.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1
{
    static class Globals
    {
        public static Agent ComplianceAgent = new Agent();
        public static Activity activity = new Activity();
        public static ChromiumWebBrowser chromePopup;
        public static string apiKey = "0a36fe1f051303b2029b25fd7a699cfcafb8e4619ddc10657ef8b32ba159e674";
        public static int LAST_SUCCESS_ID;
        public static int FIVE_MINUTES_IDLE_TIME = 600;//Seconds
        public static DateTime _wentIdle;
        public static int _idleTicks;
        public static bool SKYPE_COMPLIANCE;
        public static List<string> UrlHistory = new List<string>();

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

        public static void AddToHistory(string url)
        {
            if (UrlHistory.Contains(url))
                return;
            if (UrlHistory.Count() > 10)
                UrlHistory.RemoveAt(0);

            UrlHistory.Add(url);
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
                case (int)LogType.UserClick:
                    logFilePath = @path + "user_click_log.txt";
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
                    log.Close();
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
        UserClick = 5,
    }

    
}

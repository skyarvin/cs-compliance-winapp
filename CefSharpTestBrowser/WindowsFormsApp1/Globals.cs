﻿using CefSharp.WinForms;
using SkydevCSTool;
using SkydevCSTool.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsApp1.Models;
using System.Threading.Tasks;
using System.Net.Sockets;
using SkydevCSTool.Class;
using System.Threading;
using SkydevCSTool.Properties;
using CefSharp.WinForms.Internals;

namespace WindowsFormsApp1
{
    static class Globals
    {
        public static List<Socket> Connections = new List<Socket>();
        public static Profile Profile { get; set; }
        public static string CurrentUrl { get; set; }
        public static bool Paired { get; set; }
        public static Int32 unixTimestamp { get; set; }
        public static bool EnableTimer { get; set; }
        public static int? LAST_GROUP_ID { get; set; }

        public static List<Profile> Profiles = new List<Profile>();
        public static string PartnerAgents = "";
        public static frmMain frmMain;
        public static frmQA FrmQA;
        public static frmSetPreferences FrmSetPreferences = new frmSetPreferences();
        public static Agent ComplianceAgent = new Agent();
        public static Activity activity = new Activity();
        public static UserAccount useraccount = new UserAccount();
        public static ChromiumWebBrowser chromeBrowser;
        public static ChromiumWebBrowser chromePopup;
        public static string apiKey = "0a36fe1f051303b2029b25fd7a699cfcafb8e4619ddc10657ef8b32ba159e674";
        public static int LAST_SUCCESS_ID;
        public static int FIVE_MINUTES_IDLE_TIME = 600;//Seconds 600
        public static DateTime _wentIdle;
        public static int _idleTicks;
        public static bool SKYPE_COMPLIANCE;
        public static int SC_THRESHOLD = 20000;
        public static List<string> UrlHistory = new List<string>();
        public static Socket Client;
        public static string MyIP;
        public static List<string> ApprovedAgents = new List<string>();
        public static bool ForceHideComliance = true;
        public static int max_room_duration = 48;
        public static int room_duration;
        public static void ShowMessage(Form parent,string Message)
        {
            frmMessage frm = new frmMessage(Message);
            frm.ShowDialog(parent);
        }

        public static DialogResult ShowMessageDialog(Form parent, string Message)
        {
            frmMessage frm = new frmMessage(Message);
            frm.ShowDialog(parent);
            return DialogResult.OK;
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
            try
            {
                string logFilePath = "";
                string path = String.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), string.Concat("/SkydevCsTool/logs/", DateTime.Now.ToString("MM-dd-yyyy"), "/"));
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
            catch { }
        }
        public static void showMessage(string s) {
            Task.Factory.StartNew(() =>
            {
                MessageBox.Show(s, "Error");

            });

        }

        public static bool IsBuddySystem()
        {
            return Globals.Client != null || ServerAsync.HasConnections();
        }

        public static string CurrentVersion()
        {
            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {
                System.Deployment.Application.ApplicationDeployment cd = System.Deployment.Application.ApplicationDeployment.CurrentDeployment;
                return string.Concat("v.", cd.CurrentVersion.ToString());
            }

            return "v.0.0.0.0";
        }

        public static Dictionary<String, String> workshifts = new Dictionary<String, String>
        {
            { "DS", "Dayshift" },
            { "MS", "Midshift" },
            { "NS", "Nightshift" }
        };

        public static void SaveActivity()
        {
            try
            {
                Globals.activity.start_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                Globals.activity.agent_id = Globals.ComplianceAgent.id;
                Globals.activity.work_date = DateTime.Now.Date.ToString("yyyy-MM-dd"); //Globals.ComplianceAgent.review_date;
                Globals.activity.Save();
            }
            catch (AggregateException e)
            {
                Globals.SaveToLogFile(e.ToString(), (int)LogType.Error);
                MessageBox.Show(String.Concat("Error connecting to Chaturbate servers", System.Environment.NewLine, "Please refresh and try again.",
                    System.Environment.NewLine, "If chaturbate/internet is NOT down and you are still getting the error, Please contact dev team"), "Error");
            }
            catch (Exception e)
            {
                Globals.SaveToLogFile(e.ToString(), (int)LogType.Error);
                MessageBox.Show(String.Concat(e.Message.ToString(), System.Environment.NewLine, "Please contact Admin."), "Error");
            }

            //Globals.activity.start_time = "";
        }

        public static void UpdateActivity()
        {
            try
            {
                if (Globals.activity.id.HasValue)
                {
                    Globals.activity.end_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    Globals.activity.Update();
                }
                else
                {
                    Globals.SaveActivity();
                }
            }
            catch (AggregateException e)
            {
                Globals.SaveToLogFile(e.ToString(), (int)LogType.Error);
                MessageBox.Show(String.Concat("Error connecting to Chaturbate servers", System.Environment.NewLine, "Please refresh and try again.",
                    System.Environment.NewLine, "If chaturbate/internet is NOT down and you are still getting the error, Please contact dev team"), "Error");
            }
            catch (Exception e)
            {
                Globals.SaveToLogFile(e.ToString(), (int)LogType.Error);
                MessageBox.Show(String.Concat(e.Message.ToString(), System.Environment.NewLine, "Please contact Admin."), "Error");
            }

            Globals.activity.start_time = "";
        }
        public static bool IsServer()
        {
            return Globals.Client == null && ServerAsync.HasConnections();
        }

        public static bool IsClient()
        {
            return Globals.Client != null;
        }
        public static bool IsPreferenceSetupValid()
        {
            List<string> selected_preferences = Profiles.Select(m => m.Preference).ToList();
            if (selected_preferences.Any(sp => sp.Contains("PT")) && selected_preferences.Any(sp => sp.Contains("CL")) && selected_preferences.Any(sp => sp.Contains("BO")))
                return true;

            return false;
        }

        public static string MissingPreference()
        {
            List<string> selected_preferences = Profiles.Select(m => m.Preference).ToList();
            List<string> missing_preferences = new List<string>();
            if (!selected_preferences.Any(sp => sp.Contains("PT")))
                missing_preferences.Add("Photos");
            if (!selected_preferences.Any(sp => sp.Contains("CL")))
                missing_preferences.Add("Chatlog");
            if (!selected_preferences.Any(sp => sp.Contains("BO")))
                missing_preferences.Add("Bio");
            return string.Join(", ", missing_preferences);
        }

        public static void BroadcastPreferenceChanges()
        {
            //Broadcast to server
            Globals.Profiles.Where(m => m.AgentID == Globals.ComplianceAgent.id).FirstOrDefault().Preference = Settings.Default.preference;
            Globals.PartnerAgents = ServerAsync.ListOfPartners();
            if (Globals.IsServer())
            {
                ServerAsync.SendToAll(new PairCommand { Action = "PARTNER_LIST", Message = Globals.PartnerAgents });
               
           
            }
            else if (Globals.IsClient())
            {
                AsynchronousClient.Send(Globals.Client, new PairCommand { Action = "UPDATE_PREFERENCE", ProfileID = Globals.ComplianceAgent.id, Preference = Settings.Default.preference });
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

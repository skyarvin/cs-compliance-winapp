using CefSharp;
using CefSharp.WinForms;
using CefSharp.WinForms.Internals;
using SkydevCSTool;
using SkydevCSTool.Class;
using SkydevCSTool.Handlers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using WindowsFormsApp1.Models;
using Timer = System.Windows.Forms.Timer;
using System.Media;
using System.Net;
using System.Threading.Tasks;
using System.Text;
using System.Net.Sockets;

namespace WindowsFormsApp1
{
    public partial class frmMain : Form
    {
        private Timer _timer;
        private int room_duration;
        private int max_room_duration = 15;
        private string LastSuccessUrl;
        private DateTime StartTime;
        private Dictionary<string, string> Actions = new Dictionary<string, string>
        {
            {Action.Violation.Value, "VR" },
            {Action.IdMissing.Value, "IM" },
            {Action.SpammerSubmit.Value, "SR" },
            {Action.RequestReview.Value, "RR" },
            {Action.Agree.Value, "BA" },
            {Action.Disagree.Value, "BD" },
            {Action.SetExpiration.Value, "SE" },
            {Action.ChangeGender.Value, "CG" },
            {Action.Approve.Value, "AP" },
            {Action.ChatReply.Value, "AP" },
        };

        private List<string> Violations = new List<string>
        {
           Action.Violation.Value,
           Action.IdMissing.Value,
           Action.SpammerSubmit.Value,
           Action.RequestReview.Value
        };

        #region Init
        public void InitializeChromium(string profile)
        {
           
            Globals.Profile = profile;
            lblProfile.Text = String.Concat("Profile: ", Globals.Profile);
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            CefSettings settings = new CefSettings();
            CefSharpSettings.LegacyJavascriptBindingEnabled = true;
            settings.CachePath = @path + "/cache/cache/";
            settings.PersistSessionCookies = true;
            if (!Cef.IsInitialized)
            {
                Cef.Initialize(settings);
            }

            Cef.GetGlobalCookieManager().SetStoragePath(@path + "/SkydevCsTool/cookies/" + profile + "/", true);

            var requestContextSettings = new RequestContextSettings();
            requestContextSettings.CachePath = @path + "/cache/cache/";
            requestContextSettings.PersistSessionCookies = true;
            requestContextSettings.PersistUserPreferences = true;
            Globals.chromeBrowser = new ChromiumWebBrowser(Url.CB_HOME);
            Globals.chromeBrowser.RequestContext = new RequestContext(requestContextSettings);
            this.pnlBrowser.Controls.Add(Globals.chromeBrowser);
            Globals.chromeBrowser.Dock = DockStyle.Fill;

            Globals.chromeBrowser.AddressChanged += Browser_AddressChanged;
            var obj = new BoundObject(Globals.chromeBrowser);
            obj.HtmlItemClicked += Obj_HtmlItemClicked;

            Globals.chromeBrowser.RegisterJsObject("bound", obj);
            Globals.chromeBrowser.FrameLoadStart += obj.OnFrameLoadStart;
            Globals.chromeBrowser.FrameLoadEnd += obj.OnFrameLoadEnd;
            Globals.chromeBrowser.MenuHandler = new MyCustomMenuHandler();
            Globals.chromeBrowser.LifeSpanHandler = new BrowserLifeSpanHandler();

            lblUser.Text = Globals.ComplianceAgent.name;
            try {
                pbImg.Load(Globals.ComplianceAgent.photo);
            }
            catch { }
        }

        private void InitializeServer()
        {
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            Globals.MyIP = ipHost.AddressList[1].ToString();

            //Globals.Server = new Server(Globals.MyIP);
            //WaitIncomingClientConnection();

            Task.Factory.StartNew(() =>
            {
                AsynchronousSocketListener.StartListening(Globals.MyIP);
            });
        }

        private void InitializeAppFolders(string profile)
        {
            // TODO : Refactor this !
            string temporary_cache_directory = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\SkydevCsTool\\temp");
            if ( !Directory.Exists(temporary_cache_directory))
            {
                Directory.CreateDirectory(temporary_cache_directory);
            }
            string temporary_cookies_directory = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\SkydevCsTool\\cookies\\", profile);
            if (!Directory.Exists(temporary_cookies_directory))
            {
                Directory.CreateDirectory(temporary_cookies_directory);
            }
        }


        public frmMain()
        {
            Globals.Profiles.Add(Globals.ComplianceAgent.profile);
            InitializeComponent();
            InitializeAppFolders(Globals.ComplianceAgent.profile);
            InitializeChromium(Globals.ComplianceAgent.profile);
            InitializeServer();
            
        }
        #endregion

        #region ActivityMonitor
        private void Timer_Expired(object sender, EventArgs e)
        {
            room_duration = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds - Globals.unixTimestamp;

            if (room_duration >= max_room_duration) {
                setHeaderColor(Color.Red, Color.DarkRed);
            }

            
            if (++Globals._idleTicks >= Globals.FIVE_MINUTES_IDLE_TIME && !string.IsNullOrEmpty(Globals.activity.start_time))
            {
                Globals.UpdateActivity();
                this.InvokeOnUiThreadIfRequired(() => Globals.ShowMessage(this));
            }
            lblCountdown.Text = room_duration.ToString();
            Console.WriteLine(string.Concat("LAPSE: ", Globals._idleTicks));
        }
        private void Application_OnIdle(object sender, EventArgs e)
        {
            Globals._wentIdle = DateTime.Now;
        }

        #endregion

        #region ChromiumBrowserEvents
        private void Browser_AddressChanged(object sender, AddressChangedEventArgs e)
        {
            this.InvokeOnUiThreadIfRequired(() =>
            {
                string sCurrAddress = e.Address;
                cmbURL.Text = sCurrAddress;
                if (sCurrAddress == Url.CB_HOME)
                    return;
                if ((sCurrAddress.Contains(string.Concat(Url.CB_COMPLIANCE_URL , "/show")) || sCurrAddress.Contains(string.Concat(Url.CB_COMPLIANCE_URL, "/photoset")) ||
                    sCurrAddress.Contains("/auth/login") || sCurrAddress.Contains("/update_expiration_date_form")) && !String.IsNullOrEmpty(sCurrAddress))
                {
                    var splitAddress = sCurrAddress.Split('#');
                    if (Globals.CurrentUrl != splitAddress[0])
                    {
                        Globals.AddToHistory(splitAddress[0]);
                        Globals.SaveToLogFile(splitAddress[0], (int)LogType.Url_Change);
                        Globals.CurrentUrl = splitAddress[0];
                        StartTime = DateTime.Now;
                        Globals.SKYPE_COMPLIANCE = false;
                        Globals.unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                        lblCountdown.Text = room_duration.ToString();
                        setHeaderColor(Color.FromArgb(45, 137, 239), Color.FromArgb(31, 95, 167));
                        Globals.LastRoomChatlog = Logger.GetLastChatlog(Globals.CurrentUrl);

                        PairCommand redirectCommand = new PairCommand { Action = "GOTO", Message = Globals.CurrentUrl };
                        if (Globals.Client != null && Globals.Client.IsConnected)
                        {
                            Globals.Client.Send(redirectCommand);
                            Globals.Client.Send(new PairCommand { Action = "REQUEST_TIME" });

                        }
                        else
                            AsynchronousSocketListener.SendToAll(redirectCommand);
                    }
                }
                else
                {
                    Globals.chromeBrowser.Load(Url.CB_COMPLIANCE_URL);
                }
            });
        }

        private void Obj_HtmlItemClicked(object sender, BoundObject.HtmlItemClickedEventArgs e)
        {
            this.InvokeOnUiThreadIfRequired(() => ProcessActionButtons(e.Id));
        }

        #endregion

        #region Form Events
        private void Form1_Load(object sender, EventArgs e)
        {
            Application.Idle += new EventHandler(Application_OnIdle);
            Globals.unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            _timer = new Timer();
            _timer.Tick += new EventHandler(Timer_Expired);
            _timer.Interval = 1000;
            _timer.Start();

            Globals.SaveToLogFile("Application START", (int)LogType.Activity);
            Globals.activity.start_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Globals.SaveActivity();

            bgWorkResync.RunWorkerAsync();

            this.Text += String.Concat(" ", Globals.CurrentVersion(), " | IP Address:", Globals.MyIP);
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cef.Shutdown();
        }
        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Globals.SaveToLogFile("Application CLOSE", (int)LogType.Activity);
            Globals.UpdateActivity();
            Application.Exit();
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            Globals.SaveToLogFile("Refresh Compliance Url", (int)LogType.Activity);
            Globals.chromeBrowser.Load(Url.CB_COMPLIANCE_URL);
            PairCommand refreshCommand = new PairCommand { Action = "REFRESH" };
            if (Globals.Client != null && Globals.Client.IsConnected)
            {
                Globals.Client.Send(refreshCommand);
            }
            else
            {
                foreach (var connection in Globals.Connections)
                {
                    AsynchronousSocketListener.Send(connection, new PairCommand { Action = "REFRESH" });
                }
            }
        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            Find(true);
        }

        private void Find(bool next)
        {
            if (!string.IsNullOrEmpty(txtSearch.Text))
            {
                Globals.chromeBrowser.Find(0, txtSearch.Text, next, false, false);
            }
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            Find(true);
        }
        private void LogoutToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Cef.GetGlobalCookieManager().DeleteCookies();
            this.Close();

        }
 

        #endregion

        #region Actions

        private void ProcessActionButtons(string element_id)
        {
            Globals.SaveToLogFile(String.Concat("Process Action: ", element_id), (int)LogType.Activity);
            string violation = Globals.myStr(Globals.chromeBrowser.EvaluateScriptAsync(@"$('#id_violation option:selected').text()").Result.Result);
            string notes = Globals.myStr(Globals.chromeBrowser.EvaluateScriptAsync(@"$('#id_description').val()").Result.Result);
            string reply = Globals.myStr(Globals.chromeBrowser.EvaluateScriptAsync(@"$('#id_reply').val()").Result.Result, "Agent Reply: ");
            if (Violations.Contains(element_id) && string.IsNullOrEmpty(notes)) return;
            if (element_id == Action.Violation.Value && string.IsNullOrEmpty(violation)) return;

            if (element_id == Action.ChatReply.Value) notes = reply;
            if (element_id == Action.SetExpiration.Value) notes = "Set ID Expiration Date";

            string followRaw = Globals.myStr(Globals.chromeBrowser.EvaluateScriptAsync(@"$('#room_info').children()[1].textContent").Result.Result);
            followRaw = new String(followRaw.Where(Char.IsDigit).ToArray());
            int followers = 0;
            if (element_id != Action.SetExpiration.Value && element_id != Action.ChangeGender.Value)
                followers = int.Parse(followRaw);

            string last_chatlog = "";
            if (element_id == Action.Approve.Value) last_chatlog = (string)Globals.chromeBrowser.EvaluateScriptAsync(@"$.trim($(`#chatlog_user .chatlog tr:first-child td.chatlog_date`).html())").Result.Result;

            var logData = new Logger
            {
                url = Globals.CurrentUrl,
                agent_id = Globals.ComplianceAgent.id.ToString(),
                action = Actions[element_id],
                remarks = String.Concat(violation, notes),
                duration = Convert.ToInt32((DateTime.Now - StartTime).TotalSeconds),
                followers = followers,
                sc = followers >= Globals.SC_THRESHOLD ? true : false,
                rr = string.IsNullOrEmpty(reply) ? false : true,
                review_date = Globals.ComplianceAgent.review_date,
                workshift = Globals.ComplianceAgent.last_workshift,
                last_chatlog = last_chatlog != "" ? last_chatlog : null
            };

            try
            {
                if (Globals.CurrentUrl == LastSuccessUrl)
                {
                    logData.id = Globals.LAST_SUCCESS_ID;
                    if(logData.id != 0)
                        logData.Update();
                }
                else
                {
                    var result = logData.Save();
                    Globals.LAST_SUCCESS_ID = result.id;
                }
            }
            catch(AggregateException e)
            {
                Globals.SaveToLogFile(e.ToString(), (int)LogType.Error);
                Globals.showMessage(String.Concat("Error connecting to Chaturbate servers", System.Environment.NewLine, "Please refresh and try again.",
                    System.Environment.NewLine, "If chaturbate/internet is NOT down and you are still getting the error, Please contact dev team"));
            }
            catch(Exception e)
            {
                Globals.SaveToLogFile(e.ToString(), (int)LogType.Error);
                Globals.showMessage(String.Concat(e.Message.ToString(), System.Environment.NewLine, "Please contact Admin."));
            }


            if (element_id == Action.RequestReview.Value || element_id == Action.SetExpiration.Value || element_id == Action.ChangeGender.Value)
                LastSuccessUrl = ""; //Clear last success
            else
                LastSuccessUrl = Globals.CurrentUrl;
        }


        #endregion

        #region Override Methods
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams handleParam = base.CreateParams;
                handleParam.ExStyle |= 0x02000000;
                return handleParam;
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            try
            {
                if (keyData == (Keys.F10))
                {

                    Globals.chromeBrowser.ShowDevTools();
                    return true;
                }
                return base.ProcessCmdKey(ref msg, keyData);
            }
            catch
            {
            }
            return false;
        }
        #endregion

        public class Action
        {
            private Action(string value) { Value = value; }

            public string Value { get; set; }

            public static Action Approve { get { return new Action("approve_button"); } }
            public static Action ChangeGender { get { return new Action("change_gender"); } }
            public static Action IdMissing { get { return new Action("id-missing"); } }
            public static Action SpammerSubmit { get { return new Action("spammer-submit"); } }
            public static Action RequestReview { get { return new Action("request-review-submit"); } }
            public static Action Violation { get { return new Action("violation-submit"); } }
            public static Action Agree { get { return new Action("agree_button"); } }
            public static Action Disagree { get { return new Action("disagree_button"); } }
            public static Action SetExpiration { get { return new Action("set_expr"); } }
            public static Action ChatReply { get { return new Action("reply_button"); } }
        }

        private void BgWorkResync_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
                MessageBox.Show("Operation was canceled");
            else if (e.Error != null)
                Globals.showMessage(e.Error.Message);
            else
                bgWorkResync.RunWorkerAsync();
        }

        private void BgWorkResync_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            BackgroundWorker helperBW = sender as BackgroundWorker;
            Resync.RetryActions();
            if (helperBW.CancellationPending)
            {
                e.Cancel = true;
            }
        }

        private void UpdateWorkactivity_Tick(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(Globals.activity.start_time))
            {
                Globals.UpdateActivity();
                Globals.activity.start_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        private bool IsConnectedToPair()
        {
            if (Globals.Server != null && Globals.Server.IsConnected)
                return true;
            else if (Globals.Client != null && Globals.Client.IsConnected)
                return true;

            return false;
        }

        private void SwitchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //switchToolStripMenuItem.Enabled = false;
            //if(IsConnectedToPair())
            //    Globals.Server.Send(new PairCommand { Action = "SWITCH", Profile = Globals.Profile }) ;
            //if (Globals.Profile == "Me")
            //    Globals.Profile = "Other";
            //else
            //    Globals.Profile = "Me";

            //SwitchCache();
            //switchToolStripMenuItem.Enabled = true;
            
           
        }

        private void SwitchCache()
        {
            this.InvokeOnUiThreadIfRequired(() =>
                {
                    this.pnlBrowser.Controls.Clear();
                    Globals.chromeBrowser.Dispose();
                    Application.DoEvents();
                    InitializeChromium(Globals.Profile);
                }
            );
        }
        private void CmbURL_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                LoadUrl(cmbURL.Text.ToString());
            }
        }

        private void CmbURL_DropDown(object sender, EventArgs e)
        {
            try
            {
                cmbURL.Items.Clear();
                foreach (var item in Globals.UrlHistory)
                {
                    cmbURL.Items.Add(item);
                }
            }
            catch { }
        }

        private void CmbURL_Click_1(object sender, EventArgs e)
        {
            
        }

        private void CmbURL_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cmbURL.SelectedItem != null)
            {
                LoadUrl(cmbURL.SelectedItem.ToString());
            }
        }

        private void LoadUrl(string url)
        {
            if (url == Url.CB_HOME)
            {
                Globals.chromeBrowser.Load(url);
                return;
            }
            if ((url.Contains(string.Concat(Url.CB_COMPLIANCE_URL, "/show")) || url.Contains(string.Concat(Url.CB_COMPLIANCE_URL, "/photoset")) ||
                url.Contains(string.Concat(Url.CB_COMPLIANCE_URL, "/auth/login")) || url.Contains(string.Concat(Url.CB_COMPLIANCE_URL, "/update_expiration_date_form"))) &&
                    !String.IsNullOrEmpty(url))
            {
                Globals.chromeBrowser.Load(url);
            }
            else
            {
                Globals.chromeBrowser.Load(Url.CB_COMPLIANCE_URL);
            }
        }

        private void setHeaderColor(Color backcolor, Color darkBackColor)
        {
            lblUser.BackColor = darkBackColor;
            cmbURL.BackColor = darkBackColor;
            cmbURL.BorderColor = darkBackColor;
            pnlSearch.BackColor = darkBackColor;
            panel1.BackColor = backcolor;
            panel2.BackColor = backcolor;
            btnFind.BackColor = backcolor;
            btnRefresh.BackColor = backcolor;
            pbImg.BackColor = backcolor;
            lblCountdown.BackColor = darkBackColor;
            lblCountdown.ForeColor = Color.White;
        }

        private void CmbURL_Resize(object sender, EventArgs e)
        {
            cmbURL.Refresh();
        }

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            frmPairConnect PairConnect = new frmPairConnect();
            PairConnect.ShowDialog(this);
            if (Globals.Client.IsConnected)
            {
                SwitchCache();
                StartListenToServer();
                Globals.Client.Send(new PairCommand { Action = "BEGIN_SEND" });
            }
        }

        private void StartListenToServer()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    PairCommand data = Globals.Client.Receive();
                    if (!string.IsNullOrEmpty(data.Action))
                    {
                        switch (data.Action)
                        {
                            case "REFRESH":
                                Globals.chromeBrowser.Load(Url.CB_COMPLIANCE_URL);
                                Globals.unixTimestamp = data.Timestamp;
                                break;
                            case "SWITCH":
                                if (data.Profile == Globals.Profile)
                                    break;
                                Globals.Profile = data.Profile;
                                Globals.unixTimestamp = data.Timestamp;
                                Byte[] bytes = Convert.FromBase64String(data.Message);
                                string temporary_cookies_directory = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\SkydevCsTool\\cookies\\", Globals.Profile);
                                if (!Directory.Exists(temporary_cookies_directory))
                                {
                                    Directory.CreateDirectory(temporary_cookies_directory);
                                }
                                string path = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\SkydevCsTool\\cookies\\", Globals.Profile, "\\Cookies");
                                File.WriteAllBytes(path, bytes);
                                SwitchCache();
                                break;
                            case "UPDATE_TIME":
                                Globals.unixTimestamp = data.Timestamp;
                                break;
                            case "GOTO":
                                if (data.Message != Globals.CurrentUrl)
                                    Globals.chromeBrowser.Load(data.Message);
                                Globals.unixTimestamp = data.Timestamp;
                                break;
                        }
                        //TODO: Services for actions
                        Console.WriteLine("Action received -> {0} ", data);
                    }
                }
            });
        }

        private void Profile_Click( object sender, EventArgs e)
        {
            if (Globals.Profile == sender.ToString())
                return;
            Globals.Profile = sender.ToString();
            foreach (var connection in Globals.Connections)
            {
                string source_path = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\SkydevCsTool\\cookies\\", Globals.Profile, "\\Cookies");
                string output_directory = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\SkydevCsTool");
                System.IO.File.Copy(source_path, String.Concat(output_directory, "\\temp\\Cookies_me"), true);
                Byte[] sbytes = File.ReadAllBytes(String.Concat(output_directory, "\\temp\\Cookies_me"));
                string file = Convert.ToBase64String(sbytes);
                AsynchronousSocketListener.Send(connection, new PairCommand { Action = "SWITCH", Profile = Globals.Profile, Message = file });
            }
            SwitchCache();
        }
        private void ContextMenuStrip1_Opened(object sender, EventArgs e)
        {
            switchToolStripMenuItem.DropDownItems.Clear();
            foreach (var profile in Globals.Profiles)
            {
                var submenu = switchToolStripMenuItem.DropDownItems.Add(profile, null, new EventHandler(Profile_Click));
                if (profile == Globals.Profile)
                    submenu.BackColor = Color.DodgerBlue;
            }

        }
        private void ContextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (Globals.Profiles.Count > 1) {
                switchToolStripMenuItem.Visible = true;
            }
        }

        private void PbImg_Click(object sender, EventArgs e)
        {
            PictureBox control = (PictureBox)sender;
            Point loc = control.PointToScreen(Point.Empty);
            contextMenuStrip1.Show(new Point(loc.X + 52, loc.Y + 41));
        }
    }
}

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
using System.Diagnostics;
using System.Reflection;
using Newtonsoft.Json;
using System.Windows.Input;
using SkydevCSTool.Properties;
using System.Security.Cryptography;
using SkydevCSTool.Models;

namespace WindowsFormsApp1
{
    public partial class frmMain : Form
    {
        private Timer _timer;
        private string LastSuccessUrl;
        private string LastSucessAction;
        private DateTime StartTime_BrowserChanged;
        private DateTime StartTime_LastAction = DateTime.Now;
        private bool isBrowserInitialized = false;
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
        public void InitializeChromium(string url)
        {

            lblProfile.Text = String.Concat("Profile: ", Globals.Profile.Name);
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            CefSettings settings = new CefSettings();
            CefSharpSettings.LegacyJavascriptBindingEnabled = true;
            settings.CachePath = @path + "/cache/cache/";
            settings.PersistSessionCookies = true;
            if (!Cef.IsInitialized)
            {
                Cef.Initialize(settings);
            }

            Cef.GetGlobalCookieManager().SetStoragePath(@path + "/SkydevCsTool/cookies/" + Globals.Profile.Name + "/", true);
            Globals.SaveToLogFile(string.Concat("Initialize Cookie: ", Globals.Profile.Name), (int)LogType.Action);
            var requestContextSettings = new RequestContextSettings();
            requestContextSettings.CachePath = @path + "/cache/cache/";
            requestContextSettings.PersistSessionCookies = true;
            requestContextSettings.PersistUserPreferences = true;

            Globals.chromeBrowser = new ChromiumWebBrowser(url);
            Globals.chromeBrowser.RequestContext = new RequestContext(requestContextSettings);
            this.pnlBrowser.Controls.Add(Globals.chromeBrowser);
            Globals.chromeBrowser.Dock = DockStyle.Fill;

            Globals.chromeBrowser.AddressChanged += Browser_AddressChanged;
            var obj = new BoundObject(Globals.chromeBrowser);
            obj.HtmlItemClicked += Obj_HtmlItemClicked;

            Globals.chromeBrowser.RegisterJsObject("bound", obj);
            Globals.chromeBrowser.FrameLoadStart += obj.OnFrameLoadStart;
            Globals.chromeBrowser.FrameLoadEnd += obj.OnFrameLoadEnd;
            Globals.chromeBrowser.IsBrowserInitializedChanged += OnIsBrowserInitiazedChanged;
            Globals.chromeBrowser.LoadingStateChanged += OnLoadingStateChanged;
            Globals.chromeBrowser.MenuHandler = new MyCustomMenuHandler();
            Globals.chromeBrowser.LifeSpanHandler = new BrowserLifeSpanHandler();
            //Globals.chromeBrowser.RequestHandler = new BrowserRequestHandler();


            lblUser.Text = Globals.ComplianceAgent.name;
            try {
                pbImg.Load(Globals.ComplianceAgent.photo);
            }
            catch { }
            Globals.EnableTimer = true;
        }

        private void InitializeServer()
        {
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            Globals.MyIP = ipHost.AddressList[1].ToString();

            Task.Factory.StartNew(() =>
            {
                ServerAsync.StartListening(Globals.MyIP);
            });
        }

        private void InitializeAppFolders()
        {
            // TODO : Refactor this !
            string temporary_cache_directory = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\SkydevCsTool\\temp");
            if (!Directory.Exists(temporary_cache_directory))
            {
                Directory.CreateDirectory(temporary_cache_directory);
            }
            string temporary_cookies_directory = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\SkydevCsTool\\cookies\\", Globals.Profile.Name);
            if (!Directory.Exists(temporary_cookies_directory))
            {
                Directory.CreateDirectory(temporary_cookies_directory);
            }
        }

        public void ClientHandleSocketError(string code)
        {
            if (code == "CON00")
            {
                this.InvokeOnUiThreadIfRequired(() => Globals.ShowMessage(this, "UNABLE TO CONNECT. PLEASE TRY AGAIN."));
                SetBtnConnectText("CONNECT");
            }

            if (code == "CON03") {
                //TODO Try to reconnect. If failed, run code below. Revert back to original profile.
                this.InvokeOnUiThreadIfRequired(() => Globals.ShowMessage(this, "SERVER CONNECTION LOST. CLICK 'CONNECT' BUTTON TO RECONNECT."));
                SetBtnConnectText("CONNECT");
                Globals.Profile = new Profile { 
                    Name = Globals.ComplianceAgent.profile, 
                    AgentID = Globals.ComplianceAgent.id,
                    Type = Server.Type,
                    Preference = Settings.Default.preference,
                    RemoteAddress = Globals.MyIP
                };
                Globals.max_room_duration = 48;
                this.InvokeOnUiThreadIfRequired(() => SwitchCache());
            }
            if (code == "CON04") {
                this.InvokeOnUiThreadIfRequired(() => Globals.ShowMessage(this, "UNABLE TO COMMUNICATE TO SERVER. PLEASE CONTACT ADMIN."));
            }
            if (Globals.Client != null)
            {
                Globals.Client.Dispose();
                Globals.Client.Close();
                Globals.Client = null;
            }
            Globals.PartnerAgents = "";
            Globals.Profiles.Clear();
            Globals.Profiles.Add(Globals.Profile);
        }

        public void ServerHandleSocketError(string code, string client = "")
        {
            if (code == "CON00")
                this.InvokeOnUiThreadIfRequired(() => Globals.ShowMessage(this, "UNABLE TO ACCEPT INCOMING CONNECTION. PLEASE CONTACT ADMIN."));

            if (code == "CON03")
                this.InvokeOnUiThreadIfRequired(() => Globals.ShowMessage(this, string.Concat("CLIENT:", client, " HAS BEEN DISCONNECTED")));

            if (code == "CON04")
                this.InvokeOnUiThreadIfRequired(() => Globals.ShowMessage(this, "THE SERVER CAN NOT SEND INFORMATION TO THE CLIENTS. PLEASE CONTACT ADMIN. "));

            if (ServerAsync.HasConnections() == false)
            {
                Globals.PartnerAgents = "";
                Globals.Profile = new Profile { 
                    Name = Globals.ComplianceAgent.profile, 
                    AgentID = Globals.ComplianceAgent.id,
                    Type = Server.Type,
                    Preference = Settings.Default.preference,
                    RemoteAddress = Globals.MyIP
                };
                Globals.Profiles.Clear();
                Globals.Profiles.Add(Globals.Profile);
                this.InvokeOnUiThreadIfRequired(() => SwitchCache());
                Globals.frmMain.SetBtnConnectText("CONNECT");
            } else
            {
                Globals.PartnerAgents = ServerAsync.ListOfPartners();
                ServerAsync.SendToAll(new PairCommand { Action = "PARTNER_LIST", Message = Globals.PartnerAgents });
            }
            
            Globals.max_room_duration = ServerAsync.DurationThreshold();
        }

        public frmMain()
        {
            InitializeServer();
            Globals.Profile = new Profile {
                Name = Globals.ComplianceAgent.profile ,
                AgentID = Globals.ComplianceAgent.id,
                Type = Server.Type,
                Preference = Settings.Default.preference,
                RemoteAddress = Globals.MyIP
            };
            Globals.Profiles.Add(Globals.Profile);
            InitializeComponent();
            InitializeAppFolders();
            InitializeChromium(Url.CB_HOME);
        }
        #endregion
        #region ActivityMonitor
        private void Timer_Expired(object sender, EventArgs e)
        {
            if (!Globals.EnableTimer)
                return;

            if (++Globals.room_duration >= Globals.max_room_duration) {
                setHeaderColor(Color.Red, Color.DarkRed);
                if (isBrowserInitialized && Globals.ForceHideComliance)
                    Globals.chromeBrowser.EvaluateScriptAsync("document.querySelectorAll('#compliance_details, #id_photos').forEach(function(el){ el.style.display = 'none'; });");
            }
            else
            {
                setHeaderColor(Color.FromArgb(45, 137, 239), Color.FromArgb(31, 95, 167));
                if (isBrowserInitialized)
                    Globals.chromeBrowser.EvaluateScriptAsync("document.querySelectorAll('#compliance_details, #id_photos').forEach(function(el){ el.style.display = 'block'; });");
            }

            if (++Globals._idleTicks >= Globals.FIVE_MINUTES_IDLE_TIME && !string.IsNullOrEmpty(Globals.activity.start_time))
            {
                Globals.UpdateActivity();
                this.InvokeOnUiThreadIfRequired(() => {
                    var result = Globals.ShowMessageDialog(this, "You have been Idle for too long");
                    if (result == DialogResult.OK)
                    {
                        Globals.SaveActivity();
                        Globals._wentIdle = DateTime.MaxValue;
                        Globals._idleTicks = 0;
                    }
                });
            }

            Console.WriteLine("INACTIVE TIME:" + WindowsActivityMonitor.GetInactiveTime());
            if (WindowsActivityMonitor.GetInactiveTime() == Globals.NO_ACTIVITY_THRESHOLD_SECONDS && !Globals.IsBuddySystem())
            {
                _timer.Stop();
                    this.InvokeOnUiThreadIfRequired(() =>
                    {
                        var result = Globals.ShowMessageDialog(this, "You have been idle for more than a minute. Your timer will reset.");
                        if (result == DialogResult.OK)
                        {
                            _timer.Start();
                            Globals.room_duration = 0;
                            this.StartTime_BrowserChanged = DateTime.Now;
                            this.WindowState = FormWindowState.Maximized;

                        }
                    });
                
            }

            lblCountdown.Text = Globals.room_duration.ToString();

        }
        private void Application_OnIdle(object sender, EventArgs e)
        {
            Globals._wentIdle = DateTime.Now;
        }

        #endregion

        #region ChromiumBrowserEvents
        private void Browser_AddressChanged(object sender, AddressChangedEventArgs e)
        {
            Console.WriteLine("BrowserChanged");
            this.InvokeOnUiThreadIfRequired(() =>
            {
                string sCurrAddress = e.Address;
                cmbURL.Text = sCurrAddress;
                if (IsValidUrl(sCurrAddress))
                {
                    var splitAddress = sCurrAddress.Split('#');
                    if (Globals.CurrentUrl != splitAddress[0])
                    {
                        //Emailer for missed seed
                        if (sCurrAddress.Contains("seed_failure") && (Globals.IsServer() || !Globals.IsBuddySystem()))
                        {
                            Emailer email = new Emailer();
                            email.subject = "Missed Seed Notification";
                            email.message = string.Concat("Url: ", sCurrAddress,
                                "\nLast Success Url: ", LastSuccessUrl,
                                "\nLast Success Id: ", Globals.LAST_SUCCESS_ID,
                                "\nUser Id: ", Globals.Profile.AgentID);
                            email.Send();
                        }

                        Globals.AddToHistory(splitAddress[0]);
                        Globals.SaveToLogFile(splitAddress[0], (int)LogType.Url_Change);
                        Globals.CurrentUrl = splitAddress[0];
                        StartTime_BrowserChanged = DateTime.Now;
                        Globals.SKYPE_COMPLIANCE = false;
                        lblCountdown.Text = Globals.room_duration.ToString();
                        setHeaderColor(Color.FromArgb(45, 137, 239), Color.FromArgb(31, 95, 167));
                        PairCommand redirectCommand = new PairCommand { Action = "GOTO", Message = Globals.CurrentUrl };
                        if (Globals.IsServer())
                        {
                            Globals.room_duration = 0;
                            Globals.max_room_duration = ServerAsync.DurationThreshold();
                            ServerAsync.SendToAll(redirectCommand);
                            ServerAsync.SendToAll(new PairCommand { Action = "UPDATE_TIME", Message = Globals.max_room_duration.ToString(), RoomDuration = Globals.room_duration });
                        }
                        else if (Globals.IsClient())
                        {
                            AsynchronousClient.Send(Globals.Client, redirectCommand);
                            AsynchronousClient.Send(Globals.Client, new PairCommand { Action = "REQUEST_TIME" });
                        }
                        else Globals.room_duration = 0;

                    }
                }
                else
                {
                        Emailer email = new Emailer();
                        email.subject = "Invalid Url Notification";
                        email.message = string.Concat("Url: ", sCurrAddress,
                            "\nUser Id: ", Globals.Profile.AgentID,
                            "\nUser name: ", Globals.Profile.Name);
                        email.Send();
                    }
                    
                         
            });
        }

        private void OnIsBrowserInitiazedChanged(object sender, IsBrowserInitializedChangedEventArgs e)
        {
            isBrowserInitialized = e.IsBrowserInitialized;
        }

        private void Obj_HtmlItemClicked(object sender, BoundObject.HtmlItemClickedEventArgs e)
        {
            this.InvokeOnUiThreadIfRequired(() => ProcessActionButtons(e.Id));
        }

        private void OnLoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            this.InvokeOnUiThreadIfRequired(() => {
                if (e.IsLoading)
                {
                    this.pnlLoader.Visible = true;
                }
                else
                {
                    this.pnlLoader.Visible = false;
                }
            });
        }

        #endregion

        #region Form Events
        private void Form1_Load(object sender, EventArgs e)
        {
            Application.Idle += new EventHandler(Application_OnIdle);
            _timer = new Timer();
            _timer.Tick += new EventHandler(Timer_Expired);
            _timer.Interval = 1000;
            _timer.Start();

            Globals.SaveToLogFile("Application START", (int)LogType.Activity);
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
            Globals.EnableTimer = false;
            Application.Exit();
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            Globals.SaveToLogFile("Refresh Compliance Url", (int)LogType.Activity);
            Globals.chromeBrowser.Load(Url.CB_COMPLIANCE_URL);
            PairCommand refreshCommand = new PairCommand { Action = "REFRESH" };
            if (Globals.IsServer())
            {
                ServerAsync.SendToAll(new PairCommand { Action = "REFRESH" });
            }
            else if (Globals.IsClient())
            {
                AsynchronousClient.Send(Globals.Client, refreshCommand);
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

        //private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode != Keys.Enter)
        //    {
        //        return;
        //    }

        //    Find(true);
        //}

        #endregion

        #region Actions

        private void ProcessActionButtons(string element_id)
        {
            Task.Factory.StartNew(() =>
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
                    Int32.TryParse(followRaw, out followers);
                string last_chatlog = "";
                string last_photo = "";
                if (element_id == Action.Approve.Value)
                {
                    last_chatlog = (string)Globals.chromeBrowser.EvaluateScriptAsync(@"$.trim($(`#chatlog_user .chatlog tr:first-child td.chatlog_date`).html())").Result.Result;
                    last_photo = (string)Globals.chromeBrowser.EvaluateScriptAsync("$(`#photos .image_container .image`).first().text().trim()").Result.Result;
                }

                var total_duration = (DateTime.Now - StartTime_LastAction).TotalSeconds;
                if ((StartTime_LastAction - StartTime_BrowserChanged).TotalSeconds > 30)
                {
                    total_duration = (DateTime.Now - StartTime_BrowserChanged).TotalSeconds;
                }

                var logData = new Logger
                {
                    url = Globals.CurrentUrl,
                    agent_id = Globals.Profile.AgentID.ToString(),
                    action = Actions[element_id],
                    remarks = String.Concat(violation, notes),
                    duration = total_duration,
                    followers = followers,
                    sc = followers >= Globals.SC_THRESHOLD ? true : false,
                    rr = string.IsNullOrEmpty(reply) ? false : true,
                    review_date = DateTime.Now.Date.ToString("yyyy-MM-dd"), //Globals.ComplianceAgent.review_date,
                    workshift = "DS",
                    last_chatlog = last_chatlog != "" ? last_chatlog : null,
                    last_photo = last_photo != "" ? last_photo : null,
                    hash = HashMembers(),
                    members = Globals.Profiles
                };

                try
                {
                    if (logData.url == LastSuccessUrl)
                    {
                        if (LastSucessAction == element_id) return;
                        logData.id = Globals.LAST_SUCCESS_ID;
                        if (logData.id != 0)
                            logData.Update();
                        else
                        {
                            var result = logData.Save();
                            Globals.LAST_SUCCESS_ID = result.id;
                        }
                    }
                    else
                    {
                        var result = logData.Save();
                        Globals.LAST_SUCCESS_ID = result.id;
                    }

                    if (element_id == Action.RequestReview.Value)
                        LastSuccessUrl = ""; //Clear last success
                    else
                        LastSuccessUrl = logData.url;

                    LastSucessAction = element_id;
                    
                }
                catch (AggregateException e)
                {
                    Globals.SaveToLogFile(e.ToString(), (int)LogType.Error);
                    Globals.showMessage(String.Concat("Error connecting to Chaturbate servers!", System.Environment.NewLine, "Please refresh and try again.",
                        System.Environment.NewLine, "If chaturbate/internet is NOT down and you are still getting the error, Please contact dev team"));
                }
                catch (Exception e)
                {
                    Globals.SaveToLogFile(e.ToString(), (int)LogType.Error);
                    Globals.showMessage(String.Concat(e.Message.ToString(), System.Environment.NewLine, "Please contact Admin."));
                }
                StartTime_LastAction = DateTime.Now;
            });
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
            if (!string.IsNullOrEmpty(Globals.activity.start_time))
            {                
                Globals.UpdateActivity();
                Globals.activity.start_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }



        public void SwitchCache()
        {
            
            isBrowserInitialized = false;
            this.pnlBrowser.Controls.Clear();
            Globals.EnableTimer = false;
            var tempBrowser = Globals.chromeBrowser;
            Globals.SaveToLogFile(string.Concat("Switch Cache: ", Globals.Profile.Name), (int)LogType.Action);
            InitializeChromium(Url.CB_COMPLIANCE_URL);

            Task.Factory.StartNew(() =>
            {
                tempBrowser.Dispose();

                while (tempBrowser.Disposing)
                    Console.WriteLine("disposing...");
                Application.DoEvents();
            });
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
            Globals.chromeBrowser.Load(url);
        }

        private bool IsValidUrl(string url)
        {
            if (Url.DEBUG)
                return true;
            if (String.IsNullOrEmpty(url))
                return false;

            if (url.Contains("chaturbate.com"))
                return true;
        
            return false;
        }

        private void setHeaderColor(Color backcolor, Color darkBackColor)
        {
            lblUser.BackColor = darkBackColor;
            cmbURL.BackColor = darkBackColor;
            cmbURL.BorderColor = darkBackColor;
            pnlSearch.BackColor = darkBackColor;
            panel1.BackColor = backcolor;
            pnlSplitter2.BackColor = backcolor;
            panel4.BackColor = backcolor;
            pnlSplitter3.BackColor = backcolor;
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
            if (string.IsNullOrEmpty(Settings.Default.preference))
            {
                Globals.ShowMessage(this,"Please set your View Preference first and Try again");
                return;
            }

            string btn_text = btnConnect.Text.ToUpper();
            if (btn_text == "CONNECT")
            {
                frmPairConnect PairConnect = new frmPairConnect();
                PairConnect.ShowDialog(this);
            }
            else if (btn_text == "DISCONNECT")
            {
                Globals.Profile = new Profile { 
                    Name = Globals.ComplianceAgent.profile, 
                    AgentID = Globals.ComplianceAgent.id,
                    Type = Server.Type,
                    Preference = Settings.Default.preference,
                    RemoteAddress = Globals.MyIP
                };
                Globals.Profiles.Clear();
                Globals.Profiles.Add(Globals.Profile);
                Globals.max_room_duration = 48;
                if (Globals.IsClient()) {
                    //TODO LOAD ORIGINAL PROFILE AFTER CLIENT DISCONNECT
                    Globals.Client.Dispose();
                    Globals.Client.Close();
                    Globals.Client = null;
                }
                else
                {
                    foreach (var handler in Globals.Connections)
                    {
                        handler.Dispose();
                        handler.Close();
                    }
                    Globals.Connections = new List<Socket>();
                    SwitchCache();
                }
                btnConnect.Text = "CONNECT";
                pnlAction.Visible = false;
            }
            Globals.ApprovedAgents.Clear();
            Globals.PartnerAgents = "";
        }
        public void SetBtnConnectText(string txt)
        {
            this.InvokeOnUiThreadIfRequired(() =>
            {
                btnConnect.Text = txt;
                pnlAction.Visible = false;
                if (txt == "DISCONNECT") {
                    pnlAction.Visible = true;
                }
            });

        }

        private void Profile_Click(object sender, EventArgs e)
        {
            if (Globals.Profile.Name == sender.ToString())
                return;
            Profile target_profile = Globals.Profiles.Find(p => p.Name == sender.ToString());
            Globals.Profile = new Profile { Name = sender.ToString(), AgentID = target_profile != null ? target_profile.AgentID : 0 };
            foreach (var connection in Globals.Connections)
            {
                string source_path = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\SkydevCsTool\\cookies\\", Globals.Profile.Name, "\\Cookies");
                string output_directory = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\SkydevCsTool");
                System.IO.File.Copy(source_path, String.Concat(output_directory, "\\temp\\Cookies_me"), true);
                Byte[] sbytes = File.ReadAllBytes(String.Concat(output_directory, "\\temp\\Cookies_me"));
                string file = Convert.ToBase64String(sbytes);
                Globals.SaveToLogFile(string.Concat("Server command switch: ", Globals.Profile.Name, " ", Globals.Profile.AgentID), (int)LogType.Action);
                ServerAsync.Send(connection, new PairCommand { Action = "SWITCH", Profile = Globals.Profile.Name, ProfileID = Globals.Profile.AgentID, Message = file });
            }
            SwitchCache();
        }
        private void ContextMenuStrip1_Opened(object sender, EventArgs e)
        {
            switchToolStripMenuItem.DropDownItems.Clear();
            foreach (var profile in Globals.Profiles)
            {
                var submenu = switchToolStripMenuItem.DropDownItems.Add(profile.Name, null, new EventHandler(Profile_Click));
                if (profile.Name == Globals.Profile.Name)
                    submenu.BackColor = Color.DodgerBlue;
            }

        }
        private void ContextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            switchToolStripMenuItem.Visible = false;
            if (ServerAsync.HasConnections() && Globals.IsServer())
            { 
                switchToolStripMenuItem.Visible = true;
            }
        }

        private void PbImg_Click(object sender, EventArgs e)
        {
            PictureBox control = (PictureBox)sender;
            Point loc = control.PointToScreen(Point.Empty);
            contextMenuStrip1.Show(new Point(loc.X + 52, loc.Y + 41));
        }

        public void ButtonClearClick()
        {
            btnClear.PerformClick();
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            BroadCastClearEvent(btnClear.Text);
        }

        public void BroadCastClearEvent(string action)
        {
            Globals.SaveToLogFile(String.Concat("BroadCastClearEvent Action:", action), (int)LogType.Activity);
            if (action == "APPROVE")
            {
                btnClear.Text = "UNAPPROVE";
                btnClear.BackColor = Color.Gray;
                if (Globals.IsServer())
                {
                    if (!Globals.ApprovedAgents.Contains(Globals.ComplianceAgent.profile))
                    {
                        Globals.ApprovedAgents.Add(Globals.ComplianceAgent.profile);
                    }
                    Globals.frmMain.DisplayRoomApprovalRate(Globals.ApprovedAgents.Count, Globals.Profiles.Count, Globals.CurrentUrl);
                    ServerAsync.SendToAll(new PairCommand { Action = "CLEARED_AGENTS", Message = Globals.ApprovedAgents.Count.ToString(), NumberofActiveProfiles = Globals.Profiles.Count , Url = Globals.CurrentUrl});
                }
                else if (Globals.IsClient())
                {
                    AsynchronousClient.Send(Globals.Client, new PairCommand { Action = "CLEAR", Profile = Globals.ComplianceAgent.profile });
                }
            }
            else
            {
                btnClear.Text = "APPROVE";
                btnClear.BackColor = Color.FromArgb(96, 169, 23);
                if (Globals.IsServer())
                {
                    if (Globals.ApprovedAgents.Contains(Globals.ComplianceAgent.profile))
                    {
                        Globals.ApprovedAgents.Remove(Globals.ComplianceAgent.profile);
                    }
                    Globals.frmMain.DisplayRoomApprovalRate(Globals.ApprovedAgents.Count, Globals.Profiles.Count, Globals.CurrentUrl);
                    ServerAsync.SendToAll(new PairCommand { Action = "CLEARED_AGENTS", Message = Globals.ApprovedAgents.Count.ToString(), NumberofActiveProfiles = Globals.Profiles.Count, Url = Globals.CurrentUrl });
                }
                else if (Globals.IsClient())
                {
                    AsynchronousClient.Send(Globals.Client, new PairCommand { Action = "UNCLEAR", Profile = Globals.ComplianceAgent.profile });
                }
            }
        }

        public void DisplayRoomApprovalRate (int number_of_approve_agents, int number_of_agents, string url)
        {
            Console.WriteLine("Peer Approval URL:" + url);
            Console.WriteLine("Peer Approval Status:" + number_of_approve_agents +"/"+number_of_agents);
            Decimal approval_percentage = ((Decimal)number_of_approve_agents / (Decimal)number_of_agents) * 100;
            this.InvokeOnUiThreadIfRequired(() =>
            {
                pbProgress.Value = (int)approval_percentage;
                lblApproveCount.Text = String.Concat(number_of_approve_agents,"/",number_of_agents);
                btnClear.Enabled = true;
                if (number_of_approve_agents == 0)
                {
                    btnClear.Text = "APPROVE";
                    btnClear.BackColor = Color.FromArgb(96, 169, 23);
                }
            });

            if (number_of_approve_agents == number_of_agents)
            {
                this.InvokeOnUiThreadIfRequired(() => btnClear.Enabled = false);
                Globals.chromeBrowser.EvaluateScriptAsync(@"
                       console.log(`Show approve button`);
                       document.getElementById(`main`).style[`background`] = `#00B159`;
                       ");
                Globals.SaveToLogFile(String.Concat("Display approval rate, Trigger auto approve:", number_of_approve_agents, "/", number_of_agents), (int)LogType.Activity);
                if (Globals.IsServer())
                {
                    Globals.chromeBrowser.EvaluateScriptAsync(@"
                    if (window.location.href.replace(location.hash, '') == '{{target_url}}'){
                        console.log('Url matched clicking approve for :{{target_url}}');
                        document.getElementById(`approve_button`).click();
                    } else {
                        console.log('Url not match not clicking {{target_url}} vs '+ window.location.href.replace(location.hash, ''));
                    }".Replace("{{target_url}}", url) );
                }
            }
            else if (number_of_approve_agents > 0) {
                Globals.SaveToLogFile(String.Concat("Display approval rate:", number_of_approve_agents, "/", number_of_agents), (int)LogType.Activity);
                Globals.chromeBrowser.EvaluateScriptAsync(@"

                     var old_bg = document.getElementById(`main`).style[`background`];
                     document.getElementById(`main`).style[`background`] = `#00B159`;
                      setTimeout(function(){
                      document.getElementById(`main`).style[`background`] = old_bg;
                        },250);
                    ");

            }
           
        }

        private void FrmMain_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == (Keys.F1))
            {
                Globals.ForceHideComliance = false;
                Globals.chromeBrowser.EvaluateScriptAsync("$(`#compliance_details,#id_photos`).show()");
                
            }
            else if (e.Control && e.KeyCode == Keys.Oemtilde )
            {
                //ButtonClearClick();
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            Globals.chromeBrowser.StopFinding(true);
            if (!string.IsNullOrEmpty(txtSearch.Text))
            {
                Globals.chromeBrowser.Find(0, txtSearch.Text, true, false, false);
            }
        }

        private string HashMembers()
        {
            if (!Globals.IsBuddySystem())
                return null;

            var final = "";
            foreach (var profile in Globals.Profiles.OrderBy(a => a.AgentID))
            {
                final += string.Concat(profile.AgentID, ":", profile.Preference,":", profile.Type, ";");
            }
            return CalculateMD5Hash(final);
        }

        public string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cef.GetGlobalCookieManager().DeleteCookies();
            this.Close();
        }
    }

}

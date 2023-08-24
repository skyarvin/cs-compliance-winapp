using CefSharp;
using CefSharp.WinForms;
using CefSharp.WinForms.Internals;
using CSTool;
using CSTool.Class;
using CSTool.Handlers;
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
using System.Net;
using System.Threading.Tasks;
using System.Text;
using System.Net.Sockets;
using System.Diagnostics;
using Newtonsoft.Json;
using CSTool.Properties;
using System.Security.Cryptography;
using CSTool.Models;
using System.Timers;

namespace WindowsFormsApp1
{
    public partial class frmMain : Form
    {
        private const int DISABLE_CLOSE_BUTTON = 0x200;
        private Timer _timer;
        private string LastSucessAction;
        public DateTime StartTime_BrowserChanged;
        public DateTime? StartTime_LastAction = null;
        public bool isBrowserInitialized = false;
        public bool send_id_checker = true;
        public IdChecker ID_CHECKER = new IdChecker();
        private List<int> scheduledCaptureTimeList = new List<int>();
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
            {Action.RequestFacePhoto.Value, "RFP" },
        };

        private List<string> Violations = new List<string>
        {
           Action.Violation.Value,
           Action.IdMissing.Value,
           Action.SpammerSubmit.Value,
           Action.RequestReview.Value,
           Action.RequestFacePhoto.Value
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

            Cef.GetGlobalCookieManager().SetStoragePath(@path + "/CsTool/cookies/" + Globals.Profile.Name + "/", true);
            this.CheckAgentSession();
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
            Globals.chromeBrowser.RequestHandler = new BrowserRequestHandler();


            lblUser.Text = Globals.ComplianceAgent.name;
            try {
                pbImg.Load(Globals.ComplianceAgent.photo);
            }
            catch { }
            Globals.EnableTimer = true;
        }

        private void CheckAgentSession()
        {
            Logger data = Logger.FetchLastAgentLog(Globals.ComplianceAgent.id);
            if (data != null)
            {
                Globals.LastActionLog = DateTime.Parse(data.actual_end_time).ToUniversalTime();
                double timediff = Math.Abs((DateTime.UtcNow - Globals.LastActionLog).TotalMinutes);
                if (timediff >= Globals.SIXTY_MINUTES_IDLE_TIME)
                {
                    Cef.GetGlobalCookieManager().DeleteCookies();
                }
            }
            else
            {
                Cef.GetGlobalCookieManager().DeleteCookies();
            }

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
            string temporary_cache_directory = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\CsTool\\temp");
            if (!Directory.Exists(temporary_cache_directory))
            {
                Directory.CreateDirectory(temporary_cache_directory);
            }
            string temporary_cookies_directory = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\CsTool\\cookies\\", Globals.Profile.Name);
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
                    RemoteAddress = Globals.MyIP,
                    IsActive = true
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
                    RemoteAddress = Globals.MyIP,
                    IsActive = true
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
                Name = Globals.ComplianceAgent.profile,
                AgentID = Globals.ComplianceAgent.id,
                Type = Server.Type,
                Preference = Settings.Default.preference,
                RemoteAddress = Globals.MyIP,
                IsActive = true
            };
            Globals.Profiles.Add(Globals.Profile);
            InitializeComponent();
            InitializeAppFolders();
            InitializeChromium(Url.CB_HOME);
            // ## Pure Local Testing with MockSite
            //InitializeChromium(string.Concat(Url.CB_HOME, Globals.ComplianceAgent.tier_level, "/show/razer"));
        }
        #endregion
        #region ActivityMonitor
        private void Timer_Expired(object sender, EventArgs e)
        {
            if (!Globals.EnableTimer)
                return;

            Task.Run(() => 
            {
                var timediff = Math.Floor(Math.Abs((DateTime.UtcNow - Globals.LastActionLog).TotalMinutes));
                if (Globals.LastActionLog != null && timediff >= Globals.SIXTY_MINUTES_IDLE_TIME && (Globals.SIXTY_MINUTES_IDLE_TIME % timediff) == 0)
                {
                    this.CheckAgentSession();
                }
            });

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

            ShowRequestPhotoAndApproveButton();

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

            if (WindowsActivityMonitor.GetInactiveTime() == Globals.NO_ACTIVITY_THRESHOLD_SECONDS && Globals.INTERNAL_RR.id == 0)
            {

                if (Globals.IsBuddySystem())
                {
                    // Set status to Inactive
                    if (Globals.IsClient())
                    {
                        AsynchronousClient.Send(Globals.Client, new PairCommand { Action = "USER_STATUS", ProfileID = Globals.ComplianceAgent.id, Message = "INACTIVE" });
                    }
                    else if (Globals.IsServer())
                    {
                        ServerAsync.ChangeUserActivityStatus(Globals.ComplianceAgent.id, false);
                    }

                    var result = Globals.ShowMessageDialog(this, "You have been idle for more than a minute.");
                    // Go back being active as ( Client/ Server )
                    if (result == DialogResult.OK)
                    {
                        //CHECK AGAIN TO ENSURE THAT CLIENT IS STILL CONNECTED TO SERVER. BECAUSE WERE NOT GUARANTEED THAT  IT IS STILL A CLIENT AT THIS POINT
                        if (Globals.IsClient())
                        {
                            AsynchronousClient.Send(Globals.Client, new PairCommand { Action = "USER_STATUS", ProfileID = Globals.ComplianceAgent.id, Message = "ACTIVE" });
                        } else if (Globals.IsServer())
                        {
                            ServerAsync.ChangeUserActivityStatus(Globals.ComplianceAgent.id, true);
                        }
                    }
                }
                else
                {
                    _timer.Stop();
                    this.InvokeOnUiThreadIfRequired(() =>
                    {
                        var result = Globals.ShowMessageDialog(this, "You have been idle for more than a minute. Your timer will reset.");
                        if (result == DialogResult.OK)
                        {
                            _timer.Start();
                            this.ResetRoomDurationTimer();
                        }
                    });
                }


            }

            lblCountdown.Text = Globals.room_duration.ToString();

        }

        public void ResetRoomDurationTimer()
        {
            Globals.room_duration = 0;
            this.StartTime_BrowserChanged = DateTime.Now;
            this.WindowState = FormWindowState.Maximized;
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
                if (IsValidUrl(sCurrAddress))
                {
                    if (!IsComplianceUrl(sCurrAddress))
                    {
                        _timer.Stop();
                        Globals.room_duration = 0;
                    }
                    else
                    {
                        _timer.Start();
                    }

                    //showIMAP();
                    var splitAddress = sCurrAddress.Split('#');
                    if (Globals.CurrentUrl == splitAddress[0])
                    {
                        if (!sCurrAddress.Contains("seed_failure"))
                        {
                            if (this.send_id_checker && IsComplianceUrl(sCurrAddress))
                            {
                                Globals.chromeBrowser.ExecuteScriptAsync(@"
                                waitUntil('#identificationproblem_button', 5000).then((element) => bound.sendToIdChecking(), (error) => console.log(error));");
                            }
                        }
                        this.send_id_checker = false;
                        return;
                    }

                    try
                    {
                        var RoomName = sCurrAddress.Replace(Url.CB_COMPLIANCE_URL, "").Split('/')[3];
                        var OldRoomName = Globals.CurrentUrl != null ? Globals.CurrentUrl.Replace(Url.CB_COMPLIANCE_URL, "").Split('/')[3] : "";
                        if (OldRoomName == RoomName)
                        {
                            this.send_id_checker = false;
                        }
                    }

                    catch
                    {
                        return;
                    }

                    //Emailer for missed seed
                    if (sCurrAddress.Contains("seed_failure") && !String.IsNullOrEmpty(Globals.LastSuccessUrl) && sCurrAddress.Contains(ExtractUsername(Globals.LastSuccessUrl)))
                    {
                        //Send to API
                        Seed seed = new Seed();
                        seed.log_id = Globals.LAST_SUCCESS_ID;
                        seed.url = sCurrAddress;
                        seed.Save();

                        Globals.LastSuccessUrl = "";
                    }
                    else
                    {
                        if (this.send_id_checker && IsComplianceUrl(sCurrAddress))
                        {
                            Globals.chromeBrowser.ExecuteScriptAsync(@"
                                waitUntil('#identificationproblem_button', 5000).then((element) => bound.sendToIdChecking(), (error) => console.log(error));");
                        }
                    }

                    if (!Globals.StartTime_LastAction.HasValue)
                    {
                        Globals.StartTime_LastAction = DateTime.Now;
                    }
                    this.send_id_checker = false;
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

                lblTierLvlBanner.Visible = false;
                cmbURL.Text = sCurrAddress;
            });
        }

        private void OnIsBrowserInitiazedChanged(object sender, IsBrowserInitializedChangedEventArgs e)
        {
            isBrowserInitialized = e.IsBrowserInitialized;
        }

        public void hideBanner()
        {
            this.InvokeOnUiThreadIfRequired(() => lblIdStatus.Visible = false);
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
        private void setScheduledCaptureTime()
        {
            DateTime now = DateTime.UtcNow;
            Random rnd = new Random();
            foreach (int value in Enumerable.Range(1, 432))
            {
                var end = now.AddMinutes(3);
                DateTime epoch = new DateTime(1970, 1, 1);
                TimeSpan startTs = now - epoch;
                TimeSpan endTs = end - epoch;
                int startRangeSinceEpoch = (int)startTs.TotalSeconds;
                int endrangeSinceEpoch = (int)endTs.TotalSeconds;
                int nextCapture = rnd.Next(startRangeSinceEpoch, endrangeSinceEpoch + 1);
                scheduledCaptureTimeList.Add(nextCapture);
                now = end;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

            Application.Idle += new EventHandler(Application_OnIdle);
            _timer = new Timer();
            _timer.Tick += new EventHandler(Timer_Expired);
            _timer.Interval = 1000;

            Globals.SaveToLogFile("Application START", (int)LogType.Activity);
            Globals.SaveActivity();
            bgWorkResync.RunWorkerAsync();

            this.Text += String.Concat(" v.", Globals.CurrentVersion(), " | IP Address:", Globals.MyIP);

            var agent_irs = InternalRequestReview.Get(new List<int>() { Globals.Profile.AgentID });
            if (agent_irs != null && agent_irs.irs.Count() > 0)
            {
                Globals.INTERNAL_RR = agent_irs.irs.First();
                Globals.frmMain.InvokeOnUiThreadIfRequired(() =>
                {
                    if (Globals.FrmInternalRequestReview == null || Globals.FrmInternalRequestReview.IsDisposed)
                        Globals.FrmInternalRequestReview = new frmInternalRequestReview();
                    StartbgWorkIRR();
                    Globals.FrmInternalRequestReview.Show();
                });
            }
            
            var agent_irfp = InternalRequestFacePhoto.Get(new List<int>() { Globals.Profile.AgentID });
            if (agent_irfp != null && agent_irfp.irfp.Count() > 0)
            {
                Globals.INTERNAL_IRFP = agent_irfp.irfp.First();
                Globals.frmMain.InvokeOnUiThreadIfRequired(() =>
                {
                    if (Globals.FrmInternalRequestFacePhoto == null || Globals.FrmInternalRequestFacePhoto.IsDisposed)
                        Globals.FrmInternalRequestFacePhoto = new frmInternalRequestFacePhoto();
                    StartbgWorkIRFP();
                    Globals.FrmInternalRequestFacePhoto.Show();
                });
            }

            bgWorkID.RunWorkerAsync();

            Globals.AnnouncementsList = Announcements.FetchAnnouncements();
            var announcement = Globals.AnnouncementsList.announcements.FindLast(x => x.read_status == false);
            if (announcement != null)
            {
                var FrmAnnouncement = new frmAnnouncement(announcement);
                FrmAnnouncement.ShowDialog();
            }

            bgWorkAnnouncement.RunWorkerAsync();
            setScheduledCaptureTime();
            System.Timers.Timer activityMonitorTimer = new System.Timers.Timer(1000);
            activityMonitorTimer.Elapsed += timer_Elapsed;
            activityMonitorTimer.Start();
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            monitorActivity();
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ModifierKeys == Keys.Alt && ModifierKeys == Keys.F4)
            {
                e.Cancel = true;
            }
            else
            {
                Cef.Shutdown();
            }
        }
        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Globals.SaveToLogFile("Application CLOSE", (int)LogType.Activity);
            Globals.UpdateActivity();
            Globals.EnableTimer = false;

            Environment.Exit(Environment.ExitCode);
            //Application.Exit();
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            this.CheckAgentSession();
            this.RefreshBrowser();
        }

        private void RefreshBrowser()
        {
            Globals.SaveToLogFile("Refresh Compliance Url", (int)LogType.Activity);
            this.send_id_checker = true;
            Globals.chromeBrowser.Load(string.Concat(Url.CB_COMPLIANCE_URL, "/", Globals.ComplianceAgent.tier_level));
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
                bool createdNew;
                var mutex = new Mutex(true, Globals.CurrentUrl, out createdNew);
                if (!createdNew)
                {
                    return;
                }

                this.send_id_checker = true;
                var urlToSave = Globals.CurrentUrl;
                Globals.SaveToLogFile(String.Concat("Process Action: ", element_id), (int)LogType.Activity);
                string violation = Globals.myStr(Globals.chromeBrowser.EvaluateScriptAsync(@"$('#id_violation option:selected').text()").Result.Result);
                string notes = Globals.myStr(Globals.chromeBrowser.EvaluateScriptAsync(@"$('#id_description').val()").Result.Result);
                string reply = Globals.myStr(Globals.chromeBrowser.EvaluateScriptAsync(@"$('#id_reply').val()").Result.Result, "Agent Reply: ");
                string remarks = String.Concat(violation, notes);
                if (!string.IsNullOrEmpty(reply))
                {
                    reply = Globals.myStr(Globals.chromeBrowser.EvaluateScriptAsync(@"$('#thread_container').html()").Result.Result) + "<div>" + reply + "</div>";
                }
                if (Violations.Contains(element_id) && element_id != Action.RequestFacePhoto.Value && string.IsNullOrEmpty(notes)) return;
                if (element_id == Action.Violation.Value && string.IsNullOrEmpty(violation)) return;
                if (!string.IsNullOrEmpty(notes) || !string.IsNullOrEmpty(violation))
                {
                    Globals.chromeBrowser.EvaluateScriptAsync(@"$('#violation-submit').prop('disabled', true);");
                    Globals.chromeBrowser.EvaluateScriptAsync(@"$('#spammer-submit').prop('disabled', true);");
                }
                if (element_id == Action.ChatReply.Value) notes = reply;
                if (element_id == Action.SetExpiration.Value) notes = "Set ID Expiration Date";
                if (element_id == Action.Approve.Value) remarks = "";

                string followRaw = Globals.myStr(Globals.chromeBrowser.EvaluateScriptAsync(@"$('#room_info').children()[2].textContent").Result.Result);
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

                var actual_start_time = Globals.StartTime_LastAction;
                if ((StartTime_BrowserChanged - (DateTime)Globals.StartTime_LastAction).TotalSeconds > 30)
                {
                    actual_start_time = StartTime_BrowserChanged;
                }

                var actual_end_time = DateTime.Now;
                var logData = new Logger
                {
                    url = urlToSave,
                    agent_id = Globals.Profile.AgentID.ToString(),
                    action = Actions[element_id],
                    remarks = remarks,
                    followers = followers,
                    sc = followers >= Globals.SC_THRESHOLD ? true : false,
                    rr = string.IsNullOrEmpty(reply.Trim()) ? false : true,
                    review_date = DateTime.Now.Date.ToString("yyyy-MM-dd"), //Globals.ComplianceAgent.review_date,
                    workshift = "DS",
                    last_chatlog = last_chatlog != "" ? last_chatlog : null,
                    last_photo = last_photo != "" ? last_photo : null,
                    actual_start_time = actual_start_time.Value.ToString("yyyy-MM-dd HH:mm:ss.ffffffzzz"),
                    actual_end_time = actual_end_time.ToString("yyyy-MM-dd HH:mm:ss.ffffffzzz"),
                    hash = HashMembers(),
                    members = Globals.Profiles,
                    is_trainee = Globals.ComplianceAgent.is_trainee
                };
                if (ExtractUsername(this.ID_CHECKER.url) == ExtractUsername(logData.url) && this.ID_CHECKER.id != 0)
                    logData.idc_id = this.ID_CHECKER.id;
                Globals.SaveToLogFile(string.Concat("IR: ", JsonConvert.SerializeObject(Globals.INTERNAL_RR)), (int)LogType.Action);
                Globals.SaveToLogFile(string.Concat("IRFP: ", JsonConvert.SerializeObject(Globals.INTERNAL_IRFP)), (int)LogType.Action);
                if (ExtractUsername(Globals.INTERNAL_RR.url) == ExtractUsername(logData.url) && Globals.INTERNAL_RR.id != 0)
                    logData.irs_id = Globals.INTERNAL_RR.id;
                else if (Globals.INTERNAL_RR.id > 0)
                {
                    Emailer email = new Emailer();
                    email.subject = "IRR Error";
                    email.message = string.Concat(JsonConvert.SerializeObject(Globals.INTERNAL_RR), "\n\r", "Current Url: ", urlToSave);
                    email.Send();
                }
                
                if (ExtractUsername(Globals.INTERNAL_IRFP.url) == ExtractUsername(logData.url) && Globals.INTERNAL_IRFP.id != 0)
                    logData.irfp_id = Globals.INTERNAL_IRFP.id;
                else if (Globals.INTERNAL_IRFP.id > 0)
                {
                    Emailer email = new Emailer();
                    email.subject = "IRFP Error";
                    email.message = string.Concat(JsonConvert.SerializeObject(Globals.INTERNAL_IRFP), "\n\r", "Current Url: ", urlToSave);
                    email.Send();
                }

                Globals.StartTime_LastAction = actual_end_time;

                try
                {
                    if (Globals.CurrentUrl == Globals.LastSuccessUrl)
                    {
                        logData.id = Globals.LAST_SUCCESS_ID;
                        if (logData.id != 0)
                        {
                            logData.Update();
                        }
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

                    if (element_id == Action.SetExpiration.Value || element_id == Action.ChangeGender.Value)
                        Globals.LastSuccessUrl = ""; //Clear last success
                    else
                        Globals.LastSuccessUrl = urlToSave;

                    this.InvokeOnUiThreadIfRequired(() =>
                    {
                        if (Globals.FrmInternalRequestReview != null)
                            Globals.FrmInternalRequestReview.Close();
                    });

                    this.InvokeOnUiThreadIfRequired(() =>
                    {
                        if (Globals.FrmInternalRequestFacePhoto != null)
                            Globals.FrmInternalRequestFacePhoto.Close();
                    });

                    if (bgWorkIRR.IsBusy)
                        bgWorkIRR.CancelAsync();
                    Globals.INTERNAL_RR = new InternalRequestReview();
                    Globals.INTERNAL_IRFP = new InternalRequestFacePhoto();
                }
                catch (AggregateException e)
                {
                    Globals.SaveToLogFile(e.ToString(), (int)LogType.Error);
                    Globals.showMessage(String.Concat("Error connecting to Compliance servers!", System.Environment.NewLine, "Please refresh and try again.",
                        System.Environment.NewLine, "If internet is NOT down and you are still getting the error, Please contact dev team"));
                }
                catch (Exception e)
                {
                    Globals.SaveToLogFile(e.ToString(), (int)LogType.Error);
                    Globals.showMessage(String.Concat(e.Message.ToString(), System.Environment.NewLine, "Please contact Admin."));
                }


                if (Globals.IsClient())
                {
                    AsynchronousClient.Send(Globals.Client, new PairCommand { Action = "UPDATE_START_TIME", Message = Globals.StartTime_LastAction.ToString() });
                }
                else
                {
                    ServerAsync.SendToAll(new PairCommand { Action = "UPDATE_START_TIME", Message = Globals.StartTime_LastAction.ToString() });
                }
                if (!Globals.CurrentUrl.Contains(Globals.ComplianceAgent.tier_level.ToString()))
                {
                    this.RefreshBrowser();
                }

                mutex.Dispose();
            });
        }

        #endregion

        #region Override Methods
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams handleParam = base.CreateParams;
                handleParam.ClassStyle = handleParam.ClassStyle | DISABLE_CLOSE_BUTTON;
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
            public static Action RequestFacePhoto { get { return new Action("request_photo_button"); } }
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
                this.send_id_checker = false;
                LoadUrl(cmbURL.Text.ToString());
            }
            else
            {
                e.Handled = true;
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
            this.send_id_checker = false;
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
            if (!IsValidUrl(url))
            {
                Emailer email = new Emailer();
                email.subject = "Invalid Url Notification";
                email.message = string.Concat("Url: ", url,
                    "\nUser Id: ", Globals.Profile.AgentID,
                    "\nUser name: ", Globals.Profile.Name);
                email.Send();
            }
            Globals.chromeBrowser.Load(url);
        }

        public bool IsValidUrl(string url)
        {
            if (Url.DEBUG)
                return true;
            if (String.IsNullOrEmpty(url))
                return false;

            if (url.Contains(Url.DOMAIN))
                return true;

            return false;
        }

        public bool IsComplianceUrl(string url)
        {
            if (url.Contains("compliance") && url.Contains("show") || url.Contains("compliance") && url.Contains("photoset"))
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
            btnKb.BackColor = backcolor;
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
                Globals.ShowMessage(this, "Please set your View Preference first and Try again");
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
                LoadOriginalProfile();
            }
            Globals.ApprovedAgents.Clear();
            Globals.PartnerAgents = "";
        }
        public void LoadOriginalProfile()
        {
            Globals.Profile = new Profile
            {
                Name = Globals.ComplianceAgent.profile,
                AgentID = Globals.ComplianceAgent.id,
                Type = Server.Type,
                Preference = Settings.Default.preference,
                RemoteAddress = Globals.MyIP,
                IsActive = true
            };
            Globals.Profiles.Clear();
            Globals.Profiles.Add(Globals.Profile);
            Globals.max_room_duration = 48;
            if (Globals.IsClient())
            {
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
            Globals.Profile = new Profile { Name = sender.ToString(), AgentID = target_profile != null ? target_profile.AgentID : 0, IsActive = true };
            foreach (var connection in Globals.Connections)
            {
                string source_path = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\CsTool\\cookies\\", Globals.Profile.Name, "\\Cookies");
                string output_directory = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\CsTool");
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
                    ServerAsync.SendToAll(new PairCommand { Action = "CLEARED_AGENTS", Message = Globals.ApprovedAgents.Count.ToString(), NumberofActiveProfiles = Globals.Profiles.Count, Url = Globals.CurrentUrl });
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

        public void DisplayRoomApprovalRate(int number_of_approve_agents, int number_of_agents, string url)
        {
            Console.WriteLine("Peer Approval URL:" + url);
            Console.WriteLine("Peer Approval Status:" + number_of_approve_agents + "/" + number_of_agents);
            Decimal approval_percentage = ((Decimal)number_of_approve_agents / (Decimal)number_of_agents) * 100;
            this.InvokeOnUiThreadIfRequired(() =>
            {
                pbProgress.Value = (int)approval_percentage;
                lblApproveCount.Text = String.Concat(number_of_approve_agents, "/", number_of_agents);
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
                    }".Replace("{{target_url}}", url));
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
                if (Globals.room_duration < Globals.max_room_duration) {
                    ShowKB();
                    return;
                }
                Globals.ForceHideComliance = false;
                Globals.chromeBrowser.EvaluateScriptAsync("$(`#compliance_details,#id_photos`).show()");

            }
            else if (e.Control && e.KeyCode == Keys.Oemtilde)
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
                final += string.Concat(profile.AgentID, ":", profile.Preference, ":", profile.Type, ";");
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

        private void btnKb_Click_1(object sender, EventArgs e)
        {
            ShowKB();
        }

        public void ShowKB()
        {
            this.InvokeOnUiThreadIfRequired(() =>
            {
                Process.Start(Url.KNOWLEDGE_BASE_URL);
            });

        }
        public void StartbgWorkIRR()
        {
            if (bgWorkIRR.IsBusy)
                return;
            bgWorkIRR.RunWorkerAsync();
        }
        public void CancelbgWorkIRR()
        {
            if (bgWorkIRR.IsBusy)
                bgWorkIRR.CancelAsync();
        }
        private void bgWorkIRR_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker helperBW = sender as BackgroundWorker;
            this.InvokeOnUiThreadIfRequired(() =>
            {
                if (Globals.INTERNAL_RR.id == 0)
                {
                    if (helperBW.CancellationPending)
                    {
                        e.Cancel = true;
                    }
                    if (Globals.FrmInternalRequestReview != null)
                        Globals.FrmInternalRequestReview.Close();
                    bgWorkIRR.CancelAsync();
                    return;
                }

                if (Globals.FrmInternalRequestReview == null || Globals.FrmInternalRequestReview.IsDisposed)
                    Globals.FrmInternalRequestReview = new frmInternalRequestReview();

                this.InvokeOnUiThreadIfRequired(() => Globals.FrmInternalRequestReview.update_info());
                if (Globals.INTERNAL_RR.status != "New" && Globals.INTERNAL_RR.status != "Processing" && Globals.INTERNAL_RR.status != "Waiting SC")
                {
                    bgWorkIRR.CancelAsync();
                    Globals.FrmInternalRequestReview.Show();
                }
            });
            Console.WriteLine("bg IRR");
            Thread.Sleep(5000);
            if (helperBW.CancellationPending)
            {
                e.Cancel = true;
            }
        }

        private void bgWorkIRR_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
                return;
            if (e.Error != null)
                Globals.showMessage(e.Error.Message);
            else
                bgWorkIRR.RunWorkerAsync();
        }
        public string ExtractUsername(string url_)
        {
            if (string.IsNullOrEmpty(url_))
                return "";
            try
            {
                var splitAddress = url_.Split('/');
                return splitAddress[5];
            }
            catch {
                return "";
            }

        }

        private void bgWorkID_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker helperBW = sender as BackgroundWorker;

            if (this.ID_CHECKER.id == 0)
            {
                //bgWorkID.CancelAsync();
                //if (helperBW.CancellationPending)
                //{
                //    e.Cancel = true;
                //}
                this.InvokeOnUiThreadIfRequired(() => lblIdStatus.Visible = false);
                return;
            }

            var idchecker = IdChecker.Get(this.ID_CHECKER.id);
            if (idchecker == null)
            {
                return;
            }

            if (ExtractUsername(Globals.CurrentUrl) != ExtractUsername(idchecker.url))
            {
                //bgWorkID.CancelAsync();
                //if (helperBW.CancellationPending)
                //{
                //    e.Cancel = true;
                //}
                this.InvokeOnUiThreadIfRequired(() => lblIdStatus.Visible = false);
                return;
            }

            this.InvokeOnUiThreadIfRequired(() =>
            {
                lblIdStatus.Visible = true;
                lblIdStatus.ForeColor = Color.Black;
                lblIdStatus.Tag = idchecker.reviewer_notes;
                switch (idchecker.status)
                {
                    case "New":
                        showIMAP();
                        lblIdStatus.BackColor = Color.FromArgb(255, 255, 192);
                        lblIdStatus.Text = "PENDING FOR ID CHECKING";
                        break;
                    case "Approve":
                        Globals.chromeBrowser.EvaluateScriptAsync("document.querySelectorAll('#approve_button').forEach(function(el){ el.style.display = 'block'; });");
                        Globals.chromeBrowser.EvaluateScriptAsync("document.querySelectorAll('#identificationproblem_button').forEach(function(el){ el.style.display = 'none'; });");
                        lblIdStatus.BackColor = Color.Green;
                        lblIdStatus.Text = string.Concat("ID APPROVED", !string.IsNullOrEmpty(idchecker.reviewer_notes) ? " (Notes): " + idchecker.reviewer_notes : "");
                        lblIdStatus.ForeColor = Color.White;
                        //bgWorkID.CancelAsync();
                        break;
                    case "Id Missing":
                        Globals.chromeBrowser.EvaluateScriptAsync("document.querySelectorAll('#identificationproblem_button').forEach(function(el){ el.style.display = 'block'; });");
                        Globals.chromeBrowser.EvaluateScriptAsync("document.querySelectorAll('#approve_button').forEach(function(el){ el.style.display = 'none'; });");
                        lblIdStatus.BackColor = Color.Red;
                        lblIdStatus.Text = string.Concat("REPORT FOR ID MISSING", !string.IsNullOrEmpty(idchecker.reviewer_notes) ? " (Notes): " + idchecker.reviewer_notes : "");
                        lblIdStatus.ForeColor = Color.White;
                        //bgWorkID.CancelAsync();
                        break;
                    case "Processing":
                        hideIMAP();
                        lblIdStatus.BackColor = Color.FromArgb(230, 126, 34);
                        lblIdStatus.Text = "PROCESSING";
                        break;
                }
            });

            Thread.Sleep(5000);
            if (helperBW.CancellationPending)
            {
                e.Cancel = true;
            }
        }

        private void bgWorkID_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
                return;
            if (e.Error != null)
                Globals.showMessage(e.Error.Message);
            else
                bgWorkID.RunWorkerAsync();
        }

        public void showIdMissing(IdChecker idChecker)
        {
            //if(idChecker.id == 0 || ExtractUsername(idChecker.url) != ExtractUsername(Globals.CurrentUrl) || !isBrowserInitialized)
            //{
            //    Globals.chromeBrowser.EvaluateScriptAsync("document.querySelectorAll('#identificationproblem_button, #approve_button').forEach(function(el){ el.style.display = 'none'; });");
            //    return;
            //}
        }

        public void showIMAP()
        {
            Globals.chromeBrowser.EvaluateScriptAsync("document.querySelectorAll('#approve_button, #identificationproblem_button').forEach(function(el){ el.style.display = 'block'; });");
        }

        public void hideIMAP()
        {
            Globals.chromeBrowser.EvaluateScriptAsync("document.querySelectorAll('#approve_button, #identificationproblem_button').forEach(function(el){ el.style.display = 'none'; });");
        }

        private void lblIdStatus_Click(object sender, EventArgs e)
        {
            if (lblIdStatus.Tag == null)
                return;
            if (!String.IsNullOrEmpty(lblIdStatus.Tag.ToString()))
            {
                if (lblIdStatus.Tag.ToString() == "Retry") {
                    Globals.CurrentUrl = "";
                    btnRefresh.PerformClick();
                    return;
                }
                Clipboard.SetText(lblIdStatus.Tag.ToString());
            }
        }


        public void SendToIdChecking()
        {
            this.InvokeOnUiThreadIfRequired(() =>
            {
                lblIdStatus.Visible = true;
                lblIdStatus.Text = "SENDING TO CHECKER";
                lblIdStatus.ForeColor = Color.Black;
                lblIdStatus.BackColor = Color.FromArgb(255, 255, 192);
            });
            IdChecker id_checker = new IdChecker();
            id_checker.agent_id = Globals.ComplianceAgent.id;
            id_checker.url = Globals.CurrentUrl;
            var result = id_checker.Save();
            this.ID_CHECKER = new IdChecker();
            if (result != null)
            {
                this.ID_CHECKER = result;
                if (bgWorkID.IsBusy)
                    return;
                bgWorkID.RunWorkerAsync();
            }
            else
            {
                this.InvokeOnUiThreadIfRequired(() =>
                {
                    lblIdStatus.Visible = true;
                    lblIdStatus.Text = "Error: Connecting to server. Click to retry.";
                    lblIdStatus.Tag = "Retry";
                });
            }

        }

        public void showTierLevelWarning(string followRaw)
        {
            followRaw = new String(followRaw.Where(Char.IsDigit).ToArray());
            int followers = 0;
            Int32.TryParse(followRaw, out followers);
            if (!Globals.ComplianceAgent.is_correct_follower(followers) && cmbURL.Items.Contains(Url.CB_COMPLIANCE_URL) && cmbURL.Items.Contains("show"))
            {
                this.InvokeOnUiThreadIfRequired(() => lblTierLvlBanner.Visible = true);
            }
        }

        private void PopulateAnnouncementList()
        {
            Globals.AnnouncementsList = Announcements.FetchAnnouncements();
            btnAnnouncement.BackgroundImage = Globals.AnnouncementsList.announcements.Find(x => x.read_status == false) != null ? Resources.announcement_unread_icon : Resources.announcement_icon;
            int AnnouncementCount = Globals.AnnouncementsList.announcements.Count;
            flpAnnouncementList.Controls.Clear();
            if (AnnouncementCount > 0)
            {
                flpAnnouncementList.Controls.Clear();
                foreach (var announcement in Globals.AnnouncementsList.announcements)
                {
                    usrCtrlAnnouncement ctrl_announcement = new usrCtrlAnnouncement(announcement);
                    ctrl_announcement.Width = AnnouncementCount > 3 ? ctrl_announcement.Width - 10 : ctrl_announcement.Width + 5;
                    flpAnnouncementList.Controls.Add(ctrl_announcement);
                }
            } else
            {
                Label lblNoAnnouncements = new Label();
                lblNoAnnouncements.AutoSize = true;
                lblNoAnnouncements.Text = "No announcements posted";
                lblNoAnnouncements.TextAlign = ContentAlignment.TopCenter;
                lblNoAnnouncements.Dock = DockStyle.Top;
                flpAnnouncementList.Controls.Add(lblNoAnnouncements);
            }
        }

        private void btnAnnouncement_Click_1(object sender, EventArgs e)
        {
            flpAnnouncementList.Visible = flpAnnouncementList.Visible == true ? false : true;
        }

        private void bgWorkAnnouncement_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker helperBW = sender as BackgroundWorker;
            this.InvokeOnUiThreadIfRequired(() =>
            {
                PopulateAnnouncementList();
            });
            Thread.Sleep(60000);
            if (helperBW.CancellationPending)
            {
                e.Cancel = true;
            }
        }

        private void bgWorkAnnouncement_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
                return;
            if (e.Error != null)
                Globals.showMessage(e.Error.Message);
            else
                bgWorkAnnouncement.RunWorkerAsync();
        }

        public string getUrlSegment()
        {
            try
            {
                Uri uri = new Uri(Globals.CurrentUrl);
                string[] segments = uri.Segments;
                return segments[2];
            }
            catch
            {
                return "";
            }
        }

        private bool isTierLevelNumeric()
        {
            return int.TryParse(getUrlSegment().TrimEnd('/'), out _);
        }

        private int getCurrentTier()
        {
            return int.Parse(getUrlSegment().TrimEnd('/'));
        }

        public void showNextTierLevelBtn()
        {
            if (isTierLevelNumeric())
            {
                bool should_show_button = getCurrentTier() != 1;
                this.InvokeOnUiThreadIfRequired(() => btnDecreaseTierLevel.Visible = should_show_button);
            }
        }

        private void btnDecreaseTierLevel_Click(object sender, EventArgs e)
        {
            this.InvokeOnUiThreadIfRequired(() => btnDecreaseTierLevel.Visible = false);
            Globals.chromeBrowser.Load(string.Concat(Url.CB_COMPLIANCE_URL, "/", getCurrentTier() - 1));
        }

        public void ShowRequestPhotoAndApproveButton()
        {
            if (Globals.LogsTabButtonClicked && Globals.room_duration >= 10)
            {
                Globals.chromeBrowser.EvaluateScriptAsync("document.querySelectorAll('#approve_button').forEach(function(el){ el.style.display = 'block'; });");
            }
        }

        private void startScreenCapture(string scFileName)
        {
            Task.Run(() =>
            {
                StaffScreenshot staffscreenshot = new StaffScreenshot();
                staffscreenshot.captureScreenshot(scFileName);
                if (!uploadImage(scFileName)) {
                    retryUpload(scFileName);
                }
            });
        }

        private void monitorActivity()
        {
            DateTime now = DateTime.UtcNow;
            TimeSpan ts = now - new DateTime(1970, 1, 1);
            int nowUnixTime = (int)ts.TotalSeconds;
            if (scheduledCaptureTimeList.Contains(nowUnixTime))
            {
                string path = String.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), string.Concat("\\CsTool\\staffcam\\", DateTime.Now.ToString("MM-dd-yyyy"), "\\"));
                FileInfo logFileInfo = new FileInfo(path);
                DirectoryInfo logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
                if (!logDirInfo.Exists) logDirInfo.Create();
                string nowStr = DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss");
                string scFileName = string.Concat(path, "sc_", nowStr, ".jpeg");
                startScreenCapture(scFileName);
                findAndUploadFailedImages();
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void retryUpload(string path)
        {
            for (int i = 0; i < 3; i++)
            {
                if (uploadImage(path))
                {
                    break;
                }

                Thread.Sleep(30000);
            }
        }

        private bool uploadImage(string path)
        {
            bool lockWasTaken = false;

            try
            {
                Monitor.Enter(path, ref lockWasTaken);

                if (Globals.activity.PostScreenshot(path))
                {
                    FileUtil.deleteFile(path);
                    return true;
                }
            }
            finally
            {
                if (lockWasTaken) Monitor.Exit(path);
            }

            return false;
        }

        private void findAndUploadFailedImages()
        {
            Task.Run(() =>
            {
                string path = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), string.Concat("\\CsTool\\staffcam\\", DateTime.Now.ToString("MM-dd-yyyy"), "\\"));

                foreach (string file in Directory.GetFiles(path, "*.jpeg"))
                {
                    retryUpload(file);
                }
            });
        }

        public void StartbgWorkIRFP()
        {
            if (bgWorkIRFP.IsBusy)
                return;
            bgWorkIRFP.RunWorkerAsync();
        }
        private void bgWorkIRFP_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker helperBW = sender as BackgroundWorker;
            this.InvokeOnUiThreadIfRequired(() =>
            {
                if (Globals.INTERNAL_IRFP.id == 0)
                {
                    if (helperBW.CancellationPending)
                    {
                        e.Cancel = true;
                    }
                    if (Globals.FrmInternalRequestFacePhoto != null)
                        Globals.FrmInternalRequestFacePhoto.Close();
                    bgWorkIRFP.CancelAsync();
                    return;
                }

                if (Globals.FrmInternalRequestFacePhoto == null || Globals.FrmInternalRequestFacePhoto.IsDisposed)
                {
                    Globals.FrmInternalRequestFacePhoto = new frmInternalRequestFacePhoto();
                }

                this.InvokeOnUiThreadIfRequired(() => Globals.FrmInternalRequestFacePhoto.update_info());
                this.CheckIRFP_status();

                if (Globals.INTERNAL_IRFP.status != "New" && Globals.INTERNAL_IRFP.status != "Processing")
                {
                    bgWorkIRFP.CancelAsync();
                    Globals.FrmInternalRequestFacePhoto.Show();
                }
            });
            Thread.Sleep(5000);
            if (helperBW.CancellationPending)
            {
                e.Cancel = true;
            }
        }

        public void CheckIRFP_status()
        {
            if (Globals.INTERNAL_IRFP.status == "Approved")
            {
                Globals.chromeBrowser.EvaluateScriptAsync("document.getElementById('pre_request_photo_button').style.display = 'block';");
            }
        }

        private void bgWorkIRFP_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
                return;
            if (e.Error != null)
                Globals.showMessage(e.Error.Message);
            else
                bgWorkIRFP.RunWorkerAsync();
        }
    }
 }
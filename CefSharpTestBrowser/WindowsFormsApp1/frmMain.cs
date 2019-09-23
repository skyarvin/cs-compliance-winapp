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
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using WindowsFormsApp1.Models;
using Timer = System.Windows.Forms.Timer;

namespace WindowsFormsApp1
{
    public partial class frmMain : Form
    {
        private Timer _timer;
        public ChromiumWebBrowser chromeBrowser;
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
        public void InitializeChromium()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            CefSettings settings = new CefSettings();
            CefSharpSettings.LegacyJavascriptBindingEnabled = true;
            settings.CachePath = @path + "/cache/cache/"; ;
            settings.PersistSessionCookies = true;
            if (!Cef.IsInitialized)
            {
                Cef.Initialize(settings);
                Cef.GetGlobalCookieManager().SetStoragePath(@path + "/cache/cookie/", true);
            }

            chromeBrowser = new ChromiumWebBrowser(Url.CB_COMPLIANCE_URL);
            this.pnlBrowser.Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;
            lblUser.Text = Globals.ComplianceAgent.name;
            try {
                pbImg.Load(Globals.ComplianceAgent.photo);
            }
            catch { }
        }

        public frmMain()
        {

            InitializeComponent();
            InitializeChromium();

            chromeBrowser.AddressChanged += Browser_AddressChanged;
            var obj = new BoundObject(chromeBrowser);
            obj.HtmlItemClicked += Obj_HtmlItemClicked;

            chromeBrowser.RegisterJsObject("bound", obj);
            chromeBrowser.FrameLoadStart += obj.OnFrameLoadStart;
            chromeBrowser.FrameLoadEnd += obj.OnFrameLoadEnd;
            chromeBrowser.MenuHandler = new MyCustomMenuHandler();
            chromeBrowser.LifeSpanHandler = new BrowserLifeSpanHandler();
        }
        #endregion

        #region ActivityMonitor
        private void Timer_Expired(object sender, EventArgs e)
        {
            if (++Globals._idleTicks >= Globals.FIVE_MINUTES_IDLE_TIME && !string.IsNullOrEmpty(Globals.activity.start_time))
            {
                Globals.UpdateActivity();
                this.InvokeOnUiThreadIfRequired(() => Globals.ShowMessage(this));
            }
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
                if ((sCurrAddress.Contains(string.Concat(Url.CB_COMPLIANCE_URL, "/show")) || sCurrAddress.Contains(string.Concat(Url.CB_COMPLIANCE_URL, "/photoset"))) &&
                    !String.IsNullOrEmpty(sCurrAddress))
                {
                    var splitAddress = sCurrAddress.Split('#');
                    if (Globals.CurrentUrl != splitAddress[0])
                    {
                        Globals.AddToHistory(splitAddress[0]);
                        Globals.SaveToLogFile(splitAddress[0], (int)LogType.Url_Change);
                        Globals.CurrentUrl = splitAddress[0];
                        StartTime = DateTime.Now;
                        Globals.SKYPE_COMPLIANCE = false;
                    }
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
            _timer = new Timer();
            _timer.Tick += new EventHandler(Timer_Expired);
            _timer.Interval = 1000;
            _timer.Start();

            Globals.SaveToLogFile("Application START", (int)LogType.Activity);
            Globals.activity.start_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Globals.SaveActivity();

            this.Text += String.Concat(" ", Globals.CurrentVersion());
            bgWorkResync.RunWorkerAsync();
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
            chromeBrowser.Load(Url.CB_COMPLIANCE_URL);
        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            Find(true);
        }

        private void Find(bool next)
        {
            if (!string.IsNullOrEmpty(txtSearch.Text))
            {
                chromeBrowser.Find(0, txtSearch.Text, next, false, false);
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

            Cef.GetGlobalCookieManager().DeleteCookies("", "");
            this.Close();

        }
        private void LblUser_MouseDown_1(object sender, MouseEventArgs e)
        {
            Label control = (Label)sender;
            Point loc = control.PointToScreen(Point.Empty);
            contextMenuStrip1.Show(new Point(loc.X + 52, loc.Y + 41));
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
            if ((url.Contains(string.Concat(Url.CB_COMPLIANCE_URL, "/show")) || url.Contains(string.Concat(Url.CB_COMPLIANCE_URL, "/photoset"))) &&
                    !String.IsNullOrEmpty(url))
            {
                chromeBrowser.Load(url);
            }
            else
            {
                chromeBrowser.Load(Url.CB_COMPLIANCE_URL);
            }
        }

        #endregion

        #region Actions

        private void ProcessActionButtons(string element_id)
        {
            Globals.SaveToLogFile(String.Concat("Process Action: ", element_id), (int)LogType.Activity);
            string violation = Globals.myStr(chromeBrowser.EvaluateScriptAsync(@"$('#id_violation option:selected').text()").Result.Result);
            string notes = Globals.myStr(chromeBrowser.EvaluateScriptAsync(@"$('#id_description').val()").Result.Result);
            string reply = Globals.myStr(chromeBrowser.EvaluateScriptAsync(@"$('#id_reply').val()").Result.Result, "Agent Reply: ");
            if (Violations.Contains(element_id) && string.IsNullOrEmpty(notes)) return;
            if (element_id == Action.Violation.Value && string.IsNullOrEmpty(violation)) return;

            if (element_id == Action.ChatReply.Value) notes = reply;
            if (element_id == Action.SetExpiration.Value) notes = "Set ID Expiration Date";

            string followRaw = Globals.myStr(chromeBrowser.EvaluateScriptAsync(@"$('#room_info').children()[1].textContent").Result.Result);
            followRaw = new String(followRaw.Where(Char.IsDigit).ToArray());
            int followers = 0;
            if (element_id != Action.SetExpiration.Value && element_id != Action.ChangeGender.Value)
                followers = int.Parse(followRaw);

            string last_chatlog = "";
            if (element_id == Action.Approve.Value) last_chatlog = (string)chromeBrowser.EvaluateScriptAsync(@"$.trim($(`#chatlog_user .chatlog tr:first-child td.chatlog_date`).html())").Result.Result;

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
            if (keyData == (Keys.F10))
            {

                chromeBrowser.ShowDevTools();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
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
    }
}

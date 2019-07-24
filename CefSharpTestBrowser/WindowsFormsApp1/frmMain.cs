using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using CefSharp.WinForms.Internals;
using SkydevCSTool.Handlers;
using UserInactivityMonitoring;
using WindowsFormsApp1.Models;
using WindowsFormsApp1.Services;

namespace WindowsFormsApp1
{
    public partial class frmMain : Form
    {
        public ChromiumWebBrowser chromeBrowser;
        private string LastSuccessUrl;
        private string CurrentUrl;
        private DateTime StartTime;
        private Point loc = new Point(0, 0);
        private string reply_message;
        private Dictionary<string, string> Actions = new Dictionary<string, string>
        {
            { "violation-submit", "VR" },
            { "id-missing", "IM" },
            { "spammer-submit", "SR" },
            { "request-review-submit", "RR" },
            { "agree_button", "BA" },
            { "disagree_button", "BD" },
            { "set_expr", "SE" },
            { "change_gender", "CG" },
            { "approve_button", "AP" },
            { "reply_button", "AP" },
        };
        private List<string> Violations = new List<string>
        {
            "violation-submit",
            "id-missing",
            "spammer-submit",
            "request-review-submit"
        };

    private IInactivityMonitor inactivityMonitor = null;

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

            chromeBrowser = new ChromiumWebBrowser(Globals.CB_COMPLIANCE_URL);
            this.pnlBrowser.Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;
            lblUser.Text = Globals.ComplianceAgent.name;
            loc = new Point(lblUser.Left, lblUser.Bottom);
            try { pbImg.Load(Globals.ComplianceAgent.photo); }
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
            chromeBrowser.FrameLoadEnd += obj.OnFrameLoadEnd;
            chromeBrowser.MenuHandler = new MyCustomMenuHandler();
            chromeBrowser.LifeSpanHandler = new BrowserLifeSpanHandler();
        }

        private void Browser_AddressChanged(object sender, AddressChangedEventArgs e)
        {
            this.InvokeOnUiThreadIfRequired(() => {
                string sCurrAddress = e.Address;
                lblUrl.Text = sCurrAddress;
                if (sCurrAddress.Contains(string.Concat(Globals.CB_COMPLIANCE_URL, "/show")) && !String.IsNullOrEmpty(sCurrAddress))
                {
                    var splitAddress = sCurrAddress.Split('#');
                    chromeBrowser.ShowDevTools();
                    if (CurrentUrl != splitAddress[0])
                    {
                        LoggerServices.SaveToLogFile(splitAddress[0], (int)LogType.Url_Change);
                        CurrentUrl = splitAddress[0];
                        StartTime = DateTime.Now;
                        reply_message = "";
                    }
                }
            });

        }

        private void Obj_HtmlItemClicked(object sender, BoundObject.HtmlItemClickedEventArgs e)
        {
            this.InvokeOnUiThreadIfRequired(() => ProcessActionButtons(e.Id));
        }

        private string myStr(object o, string label = "") {
            try
            {
                if (o.ToString() != "--" && !string.IsNullOrEmpty(o.ToString()))
                    return string.Concat(label, o.ToString(), System.Environment.NewLine, " ");
                else
                    return "";
            }
            catch {
                return "";
            }
        }
        
        private void ProcessActionButtons(string element_id) {
            LoggerServices.SaveToLogFile(String.Concat("Process Action: ", element_id) , (int)LogType.Activity);
            string violation = myStr(chromeBrowser.EvaluateScriptAsync(@"$('#id_violation option:selected').text()").Result.Result);
            string notes = myStr(chromeBrowser.EvaluateScriptAsync(@"$('#id_description').val()").Result.Result);
            if (Violations.Contains(element_id) && string.IsNullOrEmpty(notes)) return;
            if (element_id == "violation-submit" && string.IsNullOrEmpty(violation)) return;

            if (element_id == "reply_button") notes = myStr(chromeBrowser.EvaluateScriptAsync(@"$('#id_reply').val()").Result.Result, "Agent Reply: ");
            if (element_id == "set_expr") notes = "Set ID Expiration Date";

            var logData = new Logger
            {
                url = CurrentUrl,
                agent_id = Globals.ComplianceAgent.id.ToString(),
                action = Actions[element_id],
                remarks = String.Concat(reply_message, violation, notes),
                duration = LoggerServices.GetDuration(StartTime)
            };

            if (CurrentUrl == LastSuccessUrl)
            {
                logData.id = Globals.LastSuccessId;
                LoggerServices.Update(logData);
            }
            else
            {
                var result = LoggerServices.Save(logData);
                Globals.LastSuccessId = result.id;
            }

            if (element_id == "request-review-submit")
                LastSuccessUrl = "requestreview";
            else
                LastSuccessUrl = CurrentUrl;
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            LoggerServices.SaveToLogFile("Application START", (int)LogType.Activity);
            //REMOVE THIS 
            //System.Windows.Forms.Clipboard.SetText("FBBAFyAw%[r{)5z?");
            inactivityMonitor = MonitorCreator.CreateInstance(MonitorType.ApplicationMonitor);
            inactivityMonitor.MonitorKeyboardEvents = true;
            inactivityMonitor.MonitorMouseEvents = true;
            inactivityMonitor.Interval = 300000; //‭300000‬
            inactivityMonitor.Elapsed += new ElapsedEventHandler(TimeElapsed);
            inactivityMonitor.Reactivated += new EventHandler(Reactivated);
            inactivityMonitor.SynchronizingObject = this;
            inactivityMonitor.Enabled = true;
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            LoggerServices.SaveToLogFile("Application CLOSE", (int)LogType.Activity);
            Cef.Shutdown();
            Application.Exit();
             
        }
        private void TimeElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                LoggerServices.SaveToLogFile(String.Concat("User Inactivity detected : ", DateTime.Now, " Total time:", (DateTime.Now - Globals.StartTime).TotalSeconds)
                    , (int)LogType.Activity);
                  this.InvokeOnUiThreadIfRequired(() => Globals.ShowMessage(this));
                //Globals.ShowMessage();
               // MessageBox.Show("Exception occured: ");
            }
            catch (Exception exception)
            {
                MessageBox.Show("Exception occured: " + exception.Message);
            }
        }

        private void Reactivated(object sender, EventArgs e)
        {
            try
            {
                Globals.StartTime = DateTime.Now;
                LoggerServices.SaveToLogFile(String.Concat("User Activity detected : ", Globals.StartTime.ToString())
                    , (int)LogType.Activity);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Exception occured: " + exception.Message);
            }
        }
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoggerServices.SaveToLogFile("Refresh Compliance Url", (int)LogType.Activity);
            chromeBrowser.Load(Globals.CB_COMPLIANCE_URL);
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

        private void BtnZoomIn_Click(object sender, EventArgs e)
        {
            //double zoomLevel = chromeBrowser.GetZoomLevelAsync();
            //chromeBrowser.SetZoomLevel(zoomLevel + 1);
        }

        private void BtnZoomOut_Click(object sender, EventArgs e)
        {

        }

        private void PbImg_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Show(lblUser, loc);
        }
        private void LogoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
          
            Cef.GetGlobalCookieManager().DeleteCookies("", "");
            this.Close();

        }
        private void LblUser_MouseDown(object sender, MouseEventArgs e)
        {
            Label control = (Label)sender;
            Point loc = control.PointToScreen(Point.Empty);
            contextMenuStrip1.Show(new Point(loc.X+52, loc.Y+41));
        }
    }

    public class BoundObject
    {
        public delegate void ItemClickedEventHandler(object sender, HtmlItemClickedEventArgs e);
        public event ItemClickedEventHandler HtmlItemClicked;
        private ChromiumWebBrowser browser;

        public BoundObject(ChromiumWebBrowser br) { browser = br; }

        
        public void OnFrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            if (e.Frame.IsMain)
            {
                browser.EvaluateScriptAsync(@"window.onmousemove = function(e) { bound.onBrowserEvent(); }");
                //browser.EvaluateScriptAsync(@"window.onclick = function(e) { bound.onBrowserEvent(); }");
                //browser.EvaluateScriptAsync(@"window.onscroll = function(e) { bound.onBrowserEvent(); }");
                if (e.Frame.Url.Contains(Globals.CB_COMPLIANCE_SET_ID_EXP_URL))
                {
                    var submit_script = @"
                        var set_expr = document.querySelectorAll(`input[value='Update Expiration Date']`)[0];
                        var expr_date = setInterval(function(){
                            if(set_expr != undefined){
                                console.log('EXPR binded');
                                set_expr.addEventListener('click', 
                                function(e)
                                {
                                    bound.onClicked('set_expr');
                                },false)
                                clearInterval(expr_date);
                            }
                        }, 1000);
                    ";
                        browser.EvaluateScriptAsync(@submit_script);
                }
                else
                {
                    var submit_script = @"
                    var violation_interval = setInterval(function(){
                        if(document.getElementById('violation-submit') != undefined){
                            console.log('VR binded');
                            document.getElementById('violation-submit').addEventListener('click', 
                            function(e)
                            {
                                bound.onClicked(e.target.id);
                            },false)
                            clearInterval(violation_interval);
                        }
                    }, 1000);
 
                    var id_missing_interval = setInterval(function(){
                        var id_missing = document.querySelectorAll(`input[value='Report Identification Missing Problem']`)[0];
                        if(id_missing != undefined){
                            console.log('IM binded');
                            id_missing.addEventListener('click', 
                            function(e)
                            {
                                bound.onClicked('id-missing');
                            },false)
                            clearInterval(id_missing_interval);
                        }
                    }, 1000);

                    var spammer_interval = setInterval(function(){
                        if($('#spammer-submit')[0] != undefined){
                            console.log('SR binded');
                            $('#spammer-submit')[0].addEventListener('click', 
                            function(e)
                            {
                                bound.onClicked(e.target.id);
                            },false)
                            clearInterval(spammer_interval);
                        }
                    }, 1000)

                    var request_review_interval = setInterval(function(){
                        if($('#request-review-submit')[0] != undefined){
                            console.log('RR binded');
                            $('#request-review-submit')[0].addEventListener('click', 
                            function(e)
                            {
                                bound.onClicked(e.target.id);
                            },false)
                            clearInterval(request_review_interval);
                        }
                    }, 1000);

                    var bs_agree = setInterval(function(){
                        if($('#agree_button')[0] != undefined){
                            console.log('BSA binded');
                            $('#agree_button')[0].addEventListener('click', 
                            function(e)
                            {
                                bound.onClicked(e.target.id);
                            },false)
                            clearInterval(bs_agree);
                        }
                    }, 1000);

                    var bs_disagree = setInterval(function(){
                        if($('#disagree_button')[0] != undefined){
                            console.log('BSD binded');
                            $('#disagree_button')[0].addEventListener('click', 
                            function(e)
                            {
                                bound.onClicked(e.target.id);
                            },false)
                            clearInterval(bs_disagree);
                        }
                    }, 1000);

                    var request_review_reply = setInterval(function(){
                        if($('#reply_button')[0] != undefined){
                            console.log('RRR binded');
                            $('#reply_button')[0].addEventListener('click', 
                            function(e)
                            {
                                bound.onClicked(e.target.id);
                            },false)
                            clearInterval(request_review_reply);
                        }
                    }, 1000);

                    var change_gender = setInterval(function(){
                        var chg = document.querySelectorAll(`input[value='Change Gender']`)[0]; 
                        if (chg != undefined){
                            console.log('CG binded');
                            chg.addEventListener('click', 
                            function(e)
                            {
                                bound.onClicked('change_gender');
                            },false)
                            clearInterval(change_gender);
                        }
                    }, 1000);

                    $('#approve_button')[0].addEventListener('click', 
                            function(e)
                            {
                                bound.onClicked(e.target.id);
                            },false)
                    
                    var bounce = $(`body:contains('locked to another bouncer')`).length;
                    if(bounce > 0){
                        bound.saveAsBounce();
                    }
                        ";
                    browser.EvaluateScriptAsync(@submit_script);
                }
            }
        }
        public void OnBrowserEvent(){

            // Application.OpenForms.Cast<Form>().Last().InvokeOnUiThreadIfRequired(() => Application.OpenForms.Cast<Form>().Last().Focus());
            try
            {
                frmMain.ActiveForm.InvokeOnUiThreadIfRequired(() => frmMain.ActiveForm.Focus());
            }
            catch {
            }

        }
        public void SaveAsBounce(){
            LoggerServices.Update(new Logger { id = Globals.LastSuccessId, action = "BN" });
        }
        public void OnClicked(string id) {          
            if (HtmlItemClicked != null)
            {
                HtmlItemClicked(this, new HtmlItemClickedEventArgs() { Id = id });
            }
        }
        public class HtmlItemClickedEventArgs : EventArgs {
            public string Id { get; set; }
        }

    }
}
